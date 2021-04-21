﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Pickups;
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

        private static LoopingFrameTimer EnemySpawnTimer = new LoopingFrameTimer(3.0f); // new InactiveLoopingFrameTimer();

        private static List<Enemy> ActiveEnemies = new List<Enemy>();
        private static EnemyPoolList EnemyPoolList { get; set; }

        private static int EnemiesSpawned { get; set; }
        public static int EnemyHealthIncrease => EnemiesSpawned;

        private static DirectorBalance Balance;

        private static int WeaponLevelsInPlay { get; set; }
        private static bool CanSpawnWeaponLevelUp => WeaponLevelsInPlay < GameConstants.MaxWeaponLevel;

        public static void Init(DirectorBalance balance)
        {
            Balance = balance;

            WeaponLevelsInPlay = 0;

            InitSpawnMechanics();

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
            TotalTime += deltaTime;

            float timeModifier = CalculateSpawnTimerModifier();

            if (EnemySpawnTimer.UpdateActivates(deltaTime * timeModifier))
                SpawnEnemy();
        }

        private static float CalculateSpawnTimerModifier()
        {
            float modifier;

            int numEnemies = ActiveEnemies.Count;

            if (numEnemies < Balance.InitialTargetEnemiesOnScreen)
                modifier = 1.5f;
            else if (numEnemies > Balance.InitialTargetEnemiesOnScreen)
                modifier = 0.5f;
            else
                modifier = 1.0f;

            return modifier;
        }


        #region Enemy Spawning

        private static ObjectPool<Enemy>[] SpawnableEnemyPools { get; set; }
        private static int HighestSpawnableIndex { get; set; }

        public static void InitSpawnMechanics()
        {
            EnemySpawnTimer.ActivateSelf();
            EnemyPoolList = PoolManager.Instance.EnemyPool;

            var pools = EnemyPoolList.GetSpawnableEnemyPools().OrderBy(x => x.ObjectPrefab.FirstSpawnMinute);
            SpawnableEnemyPools = pools.ToArray();

            HighestSpawnableIndex = 0;
            AdjustHighestSpawnableIndex();

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

        public static void SpawnEnemy()
        {
            //var enemy = EnemyPoolList.GetRandomEnemy();

            AdjustHighestSpawnableIndex();

            int spawnPoolIndex = RandomUtil.Int(HighestSpawnableIndex);
            var pool = SpawnableEnemyPools[spawnPoolIndex];

            var enemy = pool.Get();
            enemy.transform.position = SpaceUtil.RandomEnemySpawnPosition(enemy);
            enemy.OnSpawn();

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

        public static void EnemySpawned(Enemy enemy)
        {
            ActiveEnemies.Add(enemy);

#if UNITY_EDITOR
            enemy.ActiveInDirector = true;
#endif
        }

        #endregion Enemy Spawning


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
            if (!ActiveEnemies.Any())
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


            float powerupMultiplier = enemy.PowerupDropChanceMultiplier;
            float spawnChance = Balance.BaseEnemyPowerupDropChance * powerupMultiplier;
            if (RandomUtil.Bool(spawnChance))
                SpawnPowerup(enemy.transform.position);

            // TODO: Handle difficulty
        }

        private static void EnemyEscaped(Enemy enemy)
        {
            // TODO: Handle difficulty
        }

        private static float WeaponLevelOverrideChance
        {
            get
            {
                int denominator = WeaponLevelsInPlay + 2;

                float chance = 1.0f / denominator;
                chance += Balance.WeaponLevelOverrideChanceFlatAddition;

                return chance;
            }
        }

        public static void SpawnPowerup(Vector3 position)
        {
            Pickup powerup;
            if(CanSpawnWeaponLevelUp && RandomUtil.Bool(WeaponLevelOverrideChance))
            {
                powerup = PoolManager.Instance.PickupPool.Get<WeaponLevelPickup>(position);
                WeaponLevelsInPlay++;
            }
            else if (RandomUtil.Bool(Balance.OneUpOverrideChance))
            {
                powerup = PoolManager.Instance.PickupPool.Get<OneUpPickup>(position);
            }
            else
            {
                powerup = PoolManager.Instance.PickupPool.GetRandomPowerup(position);
            }
            powerup.OnSpawn();
        }

        public static void WeaponLevelPickupDestructed()
        {
            WeaponLevelsInPlay--;
        }


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
