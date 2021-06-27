using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.DirectorHelpers;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.Pickups;
using Assets.Scenes.MainMenu;
using Assets.UI;
using Assets.Util;
using UnityEngine;

namespace Assets
{
    /// <summary>
    /// The entity that directs the flow of the game, including enemy spawns,
    /// difficulty curves, powerup drops, etc.
    /// </summary>
    public static class Director
    {
        public static float TotalTime { get; private set; }
        public static float RetributionTimeScale { get; private set; } = 1.0f;
        public static bool RetributionActive => RetributionTimeScale != 1.0f;

        private static DirectorBalance Balance;

        #region Difficulty

        private static float CurrentDifficulty => DifficultyRatio.CurrentValue * DifficultyScaleRatio;

        private static BalancedRatio DifficultyRatio { get; set; }
        private static float DifficultyScaleRatio { get; set; }

        #region Difficulty Scales

        private const float DifficultyScalePerDifficultyLevel = 0.25f;

        // DifficultyScale goes from 0 to 4 inclusive.
        // DifficultyScaleRatio can therefore be 0, 0.25f, 0.5f, 0.75f, or 1f.
        private static DifficultyScale CurrentDifficultyScale
        {
            set => DifficultyScaleRatio = ((int)value) * DifficultyScalePerDifficultyLevel;
        }
        #endregion Difficulty Scales

        #endregion Difficulty

        private static ExperienceManager Exp;

        private static int EnemiesKilledSinceLastSpawn { get; set; }
        private static FrameTimer TargetTimeToKill { get; set; }
        private static LoopingFrameTimer EnemySpawnTimer { get; set; }

        private static float TargetEnemiesOnScreen { get; set; }
        private static float TargetEnemiesIncreasePerSecond { get; set; }
        //private static ApplyFloatValueOverTime SpawnRateRamp { get; set; }

        private static List<Enemy> ActiveEnemies = new List<Enemy>();
        private static int ActiveTrackedEnemiesCount { get; set; }
        private static EnemyPoolList EnemyPoolList { get; set; }

        private static int EnemiesSpawned { get; set; }
        public static int EnemyHealthIncrease => (int) (EnemiesSpawned * 1.5f);

        private static int WeaponLevelsInPlay { get; set; }
        private static bool CanSpawnWeaponLevelUp => WeaponLevelsInPlay < GameConstants.MaxWeaponLevel;

        private static bool CheckForVictim { get; set; }
        public static bool StartCheckingForVictim() => CheckForVictim = true;

        public static void Init(DirectorBalance balance)
        {
            Balance = balance;
            CurrentDifficultyScale = MainMenuManager.SelectedDifficultyScale;
            EnemiesKilledSinceLastSpawn = 0;
            EnemySpawnTimer = new LoopingFrameTimer(Balance.SpawnRate.InitialSpawnTime); // new InactiveLoopingFrameTimer();
            TargetTimeToKill = new FrameTimer(2f * EnemySpawnTimer.ActivationInterval);

            ActiveEnemies.Clear();
            ActiveTrackedEnemiesCount = 0;
            EnemiesSpawned = 0;
            TotalTime = 0;
            RetributionTimeScale = 1.0f;
            WeaponLevelsInPlay = 0;
            CheckForVictim = false;

            float initialDifficuly = Balance.Difficuly.InitialDifficultyRatio;
            float difficultyStep = Balance.Difficuly.DifficultyRatioStep;
            DifficultyRatio = new BalancedRatio(initialDifficuly, difficultyStep);

            Exp = new ExperienceManager(Balance, DifficultyScaleRatio);

            InitSpawnMechanics();

            DebugUI.SetDebugLabel("Difficulty", () => $"({DifficultyScaleRatio} x ) {CurrentDifficulty}");
            DebugUI.SetDebugLabel("Exp", () => Exp.DebugLabel);
            DebugUI.SetDebugLabel("TargetEnemies", () => TargetEnemiesOnScreen);
            DebugUI.SetDebugLabel("Spawn Rate", () => CalculateSpawnTimerModifier());
            //DebugUI.SetDebugLabel("Weapon Levels", () => $"{WeaponLevelsInPlay} {CanSpawnWeaponLevelUp} {WeaponLevelOverrideChance}");
        }


        private static string DebugActiveEnemiesString()
        {
            var countedEnemies = ActiveEnemies.Where(x => x.InfluencesDirectorGameBalance).ToList();
            string enemies = String.Join("\r\n", countedEnemies);
            string message = $"{countedEnemies.Count} {enemies}";
            return message;
        }

        public static void RunFrame(float deltaTime, float realDeltaTime)
        {
            deltaTime *= RetributionTimeScale;

            TotalTime += deltaTime;

            TargetEnemiesOnScreen += TargetEnemiesIncreasePerSecond * deltaTime;
            //SpawnRateRamp.RunFrame(deltaTime);
            TargetTimeToKill.Increment(deltaTime);
            float timeModifier = CalculateSpawnTimerModifier();

            if (EnemySpawnTimer.UpdateActivates(deltaTime * timeModifier))
            {
                SpawnEnemy();
            }

            //float xp = DebugUI.Instance.DebugTextBox.GetFloat(0);

            //if(xp != 0)
            //{
            //    var expBar = ReflectionUtil.GetMember<ProgressBar>(Exp, "ExpBar");
            //    expBar.CurrentValue = xp;
            //}
        }

        public static void SetRetributionTimeScale(RetributionBullet bullet)
        {
            RetributionTimeScale = bullet.RetributionTimeScaleValue;
        }


        #region Enemy Spawning

        private static ObjectPool<Enemy>[] SpawnableEnemyPools { get; set; }
        private static int HighestSpawnableIndex { get; set; }

        // Use weighted random to spawn enemy every other spawn
        public static bool WeightedSpawnToggle { get; set; }

        private static void InitSpawnMechanics()
        {
            // On mobile, DebugEnemy deactivation will activate the spawn timer for us.
#if UNITY_EDITOR
            EnemySpawnTimer.ActivateSelf();
#endif
            EnemyPoolList = PoolManager.Instance.EnemyPool;

            var pools = EnemyPoolList.GetSpawnableEnemyPools().OrderBy(x => x.ObjectPrefab.FirstSpawnMinute);
            SpawnableEnemyPools = pools.ToArray();

            HighestSpawnableIndex = 0;
            AdjustHighestSpawnableIndex();

            TargetEnemiesOnScreen = Balance.SpawnRate.InitialTargetEnemiesOnScreen;
            TargetEnemiesIncreasePerSecond = 1f / Balance.SpawnRate.SecondsUntilTargetEnemyIncrease;
            //InitSpawnClamp();

            #region // Dirty spawn info debug UI
            //DebugUI.SetDebugLabel("Enemy Spawn", () =>
            //{
            //    int index = HighestSpawnableIndex - 1;
            //    float nextSpawn;
            //    string nextSpawnName;
            //    if (HighestSpawnableIndex < SpawnableEnemyPools.Length)
            //    {
            //        nextSpawn = NextSpawnableEnemy.FirstSpawnMinute;
            //        nextSpawnName = NextSpawnableEnemy.name;
            //    }
            //    else
            //    {
            //        nextSpawn = -1;
            //        nextSpawnName = "N/A";
            //    }
            //    return $"{HighestSpawnableIndex}/{SpawnableEnemyPools.Length} {SpawnableEnemyPools[index].ObjectPrefab.FirstSpawnMinute} {SpawnableEnemyPools[index].ObjectPrefab.name}\r\nNEXT: {nextSpawn} {nextSpawnName}\r\n{(TotalTime / 60.0f).ToString("0.00")} ({TotalTime.ToString("0.00")})";
            //});
            #endregion
        }

        //private static void InitSpawnClamp()
        //{
        //    Action<float> SetTarget = x => TargetEnemiesOnScreen = x;
        //    float targetStart = Balance.SpawnRate.InitialTargetEnemiesOnScreen;
        //    float targetEnd = Balance.SpawnRate.FinalTargetEnemiesOnScreen;
        //    float clampDuration = Balance.SpawnRate.TargetEnemiesOnScreenRampSeconds;

        //    SpawnRateRamp = new ApplyFloatValueOverTime(null, SetTarget, targetStart, targetEnd, clampDuration);
        //    TargetEnemiesOnScreen = targetStart;
        //}

        public static void SpawnEnemy()
        {
            //var enemy = EnemyPoolList.GetRandomEnemy();

            AdjustHighestSpawnableIndex();

            int spawnPoolIndex;
            if (WeightedSpawnToggle)
                spawnPoolIndex = RandomUtil.Int(HighestSpawnableIndex);
            else
                spawnPoolIndex = WeightedRandomUtil.IndexAroundPeakRatio(HighestSpawnableIndex, CurrentDifficulty);

            WeightedSpawnToggle = !WeightedSpawnToggle;

            var pool = SpawnableEnemyPools[spawnPoolIndex];

            var enemy = pool.Get();
            enemy.transform.position = SpaceUtil.RandomEnemySpawnPosition(enemy);
            enemy.OnSpawn();

            if (CheckForVictim && GameManager.Instance.VictimWasAutomatic)
                enemy.IsVictim = true;

            if (EnemiesKilledSinceLastSpawn > 1)
            {
                DifficultyRatio.IncreaseRatio("EnemiesKilledSinceLastSpawn > 1");
            }
            EnemiesKilledSinceLastSpawn = 0;

            EnemiesSpawned++;
        }

        private static Enemy NextSpawnableEnemy => SpawnableEnemyPools[HighestSpawnableIndex].ObjectPrefab;
        private static void AdjustHighestSpawnableIndex()
        {
            if (HighestSpawnableIndex >= SpawnableEnemyPools.Length)
                return;

            float totalTimeInMinutes = TotalTime / TimeConstants.OneMinute;

            while (HighestSpawnableIndex < SpawnableEnemyPools.Length
                && totalTimeInMinutes >= NextSpawnableEnemy.FirstSpawnMinute)
            {
                HighestSpawnableIndex++;
            }
        }

        private static float CalculateSpawnTimerModifier()
        {
            float modifier;

            int numEnemies = ActiveTrackedEnemiesCount;

            //if (numEnemies < TargetEnemiesOnScreen)
                //modifier = 1.5f;
            if (numEnemies > TargetEnemiesOnScreen)
                modifier = 0.5f;
            else
                modifier = 1.0f;

            //modifier *= SpawnRateClamp;

            // Adjust modifier by a value [0.5f, 1.5f] depending on difficulty
            float difficultyModifier = 0.5f + CurrentDifficulty;
            modifier *= difficultyModifier;

            return modifier;
        }

        // Called from Enemies themselves
        public static void EnemySpawned(Enemy enemy)
        {
            ActiveEnemies.Add(enemy);
            ActiveTrackedEnemiesCount += enemy.ActiveTrackedEnemiesCountContribution;

#if UNITY_EDITOR
            enemy.ActiveInDirector = true;
#endif
        }

        #endregion Enemy Spawning


        #region Enemy Deactivation

        public static void EnemyDeactivated(Enemy enemy)
        {
#if UNITY_EDITOR
            if(!enemy.ActiveInDirector)
            {
                const string Message = "ERROR: DEACTIVATING ENEMY THAT WAS NOT ACCOUNTED FOR BY DIRECTOR.";
                enemy.Log(Message);
                GameManager.Instance.CreateFleetingText(Message, enemy.transform.position);
            }

            enemy.ActiveInDirector = false;
            enemy.DespawnHandledByDirector = true;
#endif

            ActiveEnemies.Remove(enemy);
            ActiveTrackedEnemiesCount -= enemy.ActiveTrackedEnemiesCountContribution;

            if (ActiveEnemies.Count < 2)
                EnemySpawnTimer.ActivateSelf();

            if (enemy.InfluencesDirectorGameBalance)
            {
                if (enemy.WasKilled)
                    EnemyKilled(enemy);
                else
                    EnemyEscaped(enemy);
            }
        }

        private static void EnemyKilled(Enemy enemy)
        {
            var points = enemy.PointValue;
            enemy.CreateFleetingTextAtCenter(points);
            Scoreboard.Instance.AddScore(points);


            if(Exp.KilledEnemyLevelsUp(enemy))
            {
                SpawnPowerup(enemy.transform.position);
            }
            //float spawnChance = Balance.EnemyDrops.BaseEnemyPowerupDropChance * enemy.;
            //if (RandomUtil.Bool(spawnChance))
            //    SpawnPowerup(enemy.transform.position);

            // TODO: Handle difficulty
            KilledDifficultyAdjust(enemy);
        }

        private static void EnemyEscaped(Enemy enemy)
        {
            // TODO: Handle difficulty
            DifficultyRatio.DecreaseRatio("Enemy escaped");
        }

        private static void KilledDifficultyAdjust(Enemy enemy)
        {
            EnemiesKilledSinceLastSpawn += enemy.ActiveTrackedEnemiesCountContribution;

            // Killed before target time - increase difficulty
            if (!TargetTimeToKill.Activated)
                DifficultyRatio.IncreaseRatio("Killed before");
            else
            {
                DifficultyRatio.DecreaseRatio("Killed after");
            }

            TargetTimeToKill.Reset();
        }

        #endregion Enemy Deactivation


        #region Powerup Drops

        private static float WeaponLevelOverrideChance
        {
            get
            {
                int denominator = WeaponLevelsInPlay + 2;

                float chance = 1.0f / denominator;
                chance += Balance.EnemyDrops.WeaponLevelOverrideChanceFlatAddition;

                return chance;
            }
        }

        public static void SpawnPowerup(Vector3 position)
        {
            Pickup powerup;
            if(CanSpawnWeaponLevelUp && RandomUtil.Bool(WeaponLevelOverrideChance))
            {
                powerup = PoolManager.Instance.PickupPool.GetRandomDefaultWeaponPowerupPickup(position);
                WeaponLevelsInPlay++;
            }
            else
            {
                powerup = PoolManager.Instance.PickupPool.GetRandomBasicPowerupPickup(position);
            }
            powerup.OnSpawn();
        }

        public static void WeaponLevelPickupDestructed()
        {
            WeaponLevelsInPlay--;
        }

        #endregion Powerup Drops


        #region Difficulty

        public static void IncreaseDifficulty()
        {
            DifficultyRatio.IncreaseRatio("NULL!");
        }

        public static void DecreaseDifficulty()
        {
            DifficultyRatio.DecreaseRatio("NULL!");
        }

        public static void ResetDifficulty()
        {
            DifficultyRatio.HalveCurrentRatio();
        }

        #endregion Difficulty


        #region Get Enemies

        // Can't use standard IEnumerable because enemies may get removed during enumeration.
        public static IEnumerable<Enemy> GetAllActiveEnemies()
        {
            for (int i = ActiveEnemies.Count - 1; i >= 0; i--)
                yield return ActiveEnemies[i];
        }

        #region TryGetRandomEnemy

        public static bool TryGetRandomEnemy(out Enemy enemy)
        {
            var ret = RandomUtil.TryGetRandomElement(ActiveEnemies, out enemy);
            return ret;
        }
        public static bool TryGetRandomEnemyExcluding(Enemy exclusion, out Enemy enemy)
        {
            var potentials = ActiveEnemies
                .Where(x => x != exclusion);
            var ret = RandomUtil.TryGetRandomElement(potentials, out enemy);
            return ret;
        }

        #endregion TryGetRandomEnemy

        #endregion Get Enemies
    }
}
