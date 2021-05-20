using System;
using Assets.Bullets;
using Assets.Bullets.EnemyBullets;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.FireStrategies.PlayerFireStrategies;
using Assets.GameTasks;
using Assets.GameTasks.GameTaskLists;
using Assets.Powerups;
using Assets.ScreenEdgeColliders;
using Assets.UI;
using Assets.UI.PowerupMenu;
using Assets.Util;
using Assets.ObjectPooling;
using UnityEngine;
using System.Diagnostics;
using Assets.ColorManagers;
using Assets.Powerups.Balance;
using UnityEngine.UI;
using Assets.FireStrategyManagers;
using System.Linq;
using System.Collections.Generic;
using Assets.Pickups;
using UnityEngine.SceneManagement;
using Assets.Background;

namespace Assets
{
    /// <inheritdoc/>
    public class GameManager : MonoBehaviour
    {
        #region Debug
        private TestingType CurrentTest = TestingType.NewPowerup;

        public static Type OverrideEnemyType => null; // DebugUtil.GetOverrideEnemyType<BasicEnemy>();

        [NonSerialized]
        public bool DebugPauseNextFrame;

        private enum TestingType
        {
            Nothing,
            NewWeapon,
            NewPowerup,
            NewEnemy
        };

        // Unused since saving last weapon and powerup between sessions
        //private bool TestingWeapon => CurrentTest == TestingType.NewWeapon;
        //private bool TestingPowerup => CurrentTest == TestingType.NewPowerup;

        private bool TestingEnemy => CurrentTest == TestingType.NewEnemy;

#if UNITY_EDITOR
        public int DefaultFireTypeIndex => SaveUtil.LastWeapon;
#else
        public int DefaultFireTypeIndex => FireStrategyIndexBasic;
#endif

        #endregion Debug


        #region Property Fields

        private Enemy _victimEnemy;
        private Enemy _metronomeEnemy;
        private float _playerFireDeltaTimeScale = 1f;

        #endregion Property Fields


        public static GameManager Instance { get; private set; }


        #region Prefab Properties

        [SerializeField]
        private PlayerFireStrategyManager _FireStrategyManager;

        [SerializeField]
        private PowerupBalanceManager PowerupBalance;

        [SerializeField]
        private DirectorBalance _DirectorBalance;

        [SerializeField]
        private ColorManager _ColorManager;

        [SerializeField]
        private LoopingBackgroundSprite _SpaceLarge;

        [SerializeField]
        private LoopingBackgroundSprite _SpaceSmall;



        #region Player Prefab Properties

        [SerializeField]
        public Player Player;

        [SerializeField]
        public Othello _Othello;

        [SerializeField]
        public int _StartingExtraLives = GameConstants.PrefabNumber;

        [SerializeField]
        private Scoreboard _Scoreboard = null;

        #endregion Player Prefab Properties


        #region Powerup Prefab Properties

        [SerializeField]
        private PowerupMenu _PowerupMenu = null;

        [SerializeField]
        private Monsoon _Monsoon = null;

        [SerializeField]
        private MonsoonSpawner _MonsoonSpawner = null;

        [SerializeField]
        private SentinelManager _SentinelManager = null;

        #endregion Powerup Prefab Properties


        #region UI Prefab Properties

        [SerializeField]
        private RepeatingSpriteBar _RemainingLivesBar = null;

        [SerializeField]
        private Notification _Notification = null;

        [SerializeField]
        private GameOverGUI _GameOverGUI = null;

        //[SerializeField]
        //private ProgressBar _ExpBar = null;

        #endregion UI Prefab Properties


        #region Debug Prefab Properties

        [SerializeField]
        public Enemy _DebugEnemy;
        [SerializeField]
        public DebugUI DebugUi;

        #endregion Debug Prefab Properties


        #region Screen Prefab Properties

        [SerializeField]
        private Destructor _Destructor = null;
        [SerializeField]
        private ScreenEdgeColliderSet _ScreenEdgeColliderSet = null;

        #endregion Screen Prefab Properties

        #region Misc Prefab Properties

        [SerializeField]
        private PoolManager _PoolManager = null;

        [SerializeField]
        private float _InitialWeaponTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _WeaponRainTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _OneUpRainTime = GameConstants.PrefabNumber;

        [SerializeField]
        private Sprite _LifeSprite = null;

        [SerializeField]
        private AudioSource _AudioSource = null;

        #endregion Misc Prefab Properties

        #endregion Prefab Properties


        #region Prefabs

        //public ProgressBar ExpBar => _ExpBar;

        #endregion Prefabs


        #region Init

        private void Awake()
        {
            Instance = this;

            // ONLY limit frame rate when playing via Unity editor
#if UNITY_EDITOR
            // VSync must be disabled to set custom framerate
            // 0 for no sync, 1 for panel refresh rate, 2 for 1/2 panel
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
#endif
        }

        private void Start()
        {
            InitWithoutDependencies();
            InitWithDependencies();
            InitIndependentColors();
            InitGame();

            //var bullet = _PoolManager.BulletPool.Get<SmiteLightningBullet>(SpaceUtil.WorldMap.Center);
            //var bullet2 = _PoolManager.BulletPool.Get<SmiteJointBullet>(SpaceUtil.WorldMap.Center + new Vector3(0f, 1f, 0));
        }

        private void InitWithoutDependencies()
        {
#if !UNITY_EDITOR
            Camera.main.orthographicSize *= 2.0f;
#endif
            SaveUtil.InitializeSave();

            SpaceUtil.Init();

            // _ColorManager is a prefab field, and doesn't need initialized.
            _PoolManager.Init(in _ColorManager);

            WeaponResetTimer = new FrameTimer(InitialWeaponTime);
            WeaponRainTimer = new LoopingFrameTimer(WeaponRainTime);
            WeaponRainTimer.TimeUntilActivation = 0.1f;

            OneUpRainTimer = new LoopingFrameTimer(OneUpRainTime);
            OneUpRainTimer.TimeUntilActivation = 5f;

            _PowerupMenu.Init();
            _PowerupMenu.transform.position += new Vector3(0, 0, 0);

            MonsoonSpawner.Instance = _MonsoonSpawner;

            VictimWasAutomatic = true;

            _GameOverGUI.Init();
            _Scoreboard.Init();

            SoundManager.Init(_AudioSource);

            NotificationManager.Init(_Notification);

#if !UNITY_EDITOR
            CanGameOver = true;
#else
            CanGameOver = false;
#endif
        }

        private void InitWithDependencies()
        {
            // Dependency: PoolManager
            _DebugEnemy.Init();
            _DebugEnemy.OnSpawn();
            EnemyHealthBar.StaticInit();
            _Monsoon.Init();
            _SentinelManager.Init();
            InitFireStrategies();
            Director.Init(_DirectorBalance);

            // Dependency: FireStrategies
            SetFireType(DefaultFireTypeIndex, skipMessage: true);

            // Dependency: SpaceUtil
            _Destructor.Init();
            _ScreenEdgeColliderSet.Init();
            _MonsoonSpawner.Init();
            //_Othello.Alpha = _ColorManager.Player.OthelloAlpha;
            _Othello.Init();
            Player.Init(in _FireStrategyManager);
            _SpaceLarge.Init();
            _SpaceSmall.Init();

            _RemainingLivesBar.Init(spritesOffsetFromTop: 2);

            // Dependency: SpaceUtil, PoolManager
            BfgBulletFallout.StaticInitColors(in _ColorManager);

            // Dependency: _PoolManager, Destructor
            _PowerupManager.Init(in PowerupBalance, _Destructor);

            // Dependency: _PowerupManager
            _PoolManager.PickupPool.InitializePowerups(_PowerupManager.AllPowerups);
            SaveUtil.InitializePowerups(_PowerupManager.AllPowerups);

            // Dependency: FireStrategies, _PowerupMenu, SaveUtil
            DebugUi.Init(FireStrategies, _PowerupMenu);

            // Dependency: DebugUi
            DebugUtil.Init(DebugUi, this);
        }

        private void InitFireStrategies()
        {
            var bulletPool = _PoolManager.BulletPool;
            T Prefab<T>() where T : PlayerBullet => bulletPool.GetPrefab<T>();

            FireStrategies = new CircularSelector<PlayerFireStrategy>
            {
                new BasicStrategy(Prefab<BasicBullet>(), in _FireStrategyManager),
                new ShotgunStrategy(Prefab<ShotgunBullet>(), in _FireStrategyManager),
                new BurstStrategy(Prefab<BurstBullet>(), in _FireStrategyManager),
                new BounceStrategy(Prefab<BounceBullet>(), in _FireStrategyManager),
                new AtomStrategy(Prefab<AtomBullet>(), in _FireStrategyManager),
                new SpreadStrategy(Prefab<SpreadBullet>(), in _FireStrategyManager),
                new FlakStrategy(Prefab<FlakBullet>(), in _FireStrategyManager),
                new TrampolineStrategy(Prefab<TrampolineBullet>(), in _FireStrategyManager),
                new WormholeStrategy(Prefab<WormholeBullet>(), in _FireStrategyManager),
                new GatlingStrategy(Prefab<GatlingBullet>(), in _FireStrategyManager),
                new BfgStrategy(Prefab<BfgBullet>(), in _FireStrategyManager),
                new OneManArmyStrategy(Prefab<OneManArmyBullet>(), in _FireStrategyManager),
                new DeadlyDiamondStrategy(Prefab<DeadlyDiamondBullet>(), in _FireStrategyManager),
            };
        }

        private void InitIndependentColors()
        {
            var defaultPlayerAdditional = _ColorManager.DefaultPlayerAdditionalColor();
            //Player.SpriteColor = _ColorManager.DefaultPlayer;
            _Othello.SpriteColor = _ColorManager.DefaultPlayerColorWithAlpha(_ColorManager.Player.OthelloAlpha);
            _Monsoon.SpriteColor = defaultPlayerAdditional;
            _MonsoonSpawner.SpriteColor = defaultPlayerAdditional;
        }

        private void InitGame()
        {
            // Game over is disabled by default in debug.
            // Start with no lives to clean up the GUI.
#if UNITY_EDITOR
            LivesLeft = 0;
#else
            LivesLeft = _StartingExtraLives;
#endif

        }

        #endregion Init

        #region Update

        private void Update()
        {
            if (DebugPauseNextFrame)
            {
                Debugger.Break();
                UnityEngine.Debug.Break();
                DebugPauseNextFrame = false;
            }

            DebugUtil.HandleInput();

            float deltaTime = Time.deltaTime;
            float playerTime = deltaTime;
            float playerFireScale = deltaTime * PlayerFireDeltaTimeScale * Player.RetributionTimeScale;

            if (Player.IsAlive)
            {
                if (CurrentFireStrategy.UpdateActivates(playerFireScale * Player.FireSpeedScale))
                {
                    FireCurrentStrategy();

                    _Othello.Fire();
                }

                UpdateFireStrategy(playerTime);
                WeaponRain(deltaTime);
                OneUpRain(deltaTime);


                float enemyTimeScale = TimeScaleManager.GetTimeScaleModifier(TimeScaleType.Enemy);
                float enemyDeltaTime = deltaTime * enemyTimeScale;
                Director.RunFrame(enemyDeltaTime, deltaTime);

                _PowerupManager.PassiveUpdate(playerFireScale, deltaTime);
                ManagedGameTasks.RunFrames(deltaTime);
            }

            NotificationManager.RunFrame(deltaTime);
        }

        private  void FireCurrentStrategy()
        {
            var bullets = CurrentFireStrategy.GetBullets(WeaponLevel, Player.FirePosition);
            FirePlayerBullets(bullets);

            SoundManager.PlayTestFire();
        }

        private void UpdateFireStrategy(float playerTime)
        {
            if (FireStrategies.Index != FireStrategyIndexBasic)
            {
                if (WeaponResetTimer.UpdateActivates(playerTime))
                    SetFireType(FireStrategyIndexBasic, skipMessage: true);
            }
        }

        private Vector3 RandomRainPosition(Pickup pickup)
        {
            Vector2 size = pickup.Size;
            float spawnX = SpaceUtil.RandomWorldPositionX(size.x);
            float spawnY = SpaceUtil.WorldMap.Top.y + (size.y * 0.5f);

            Vector3 ret = new Vector3(spawnX, spawnY);
            return ret;
        }

        private void WeaponRain(float deltaTime)
        {
            if (WeaponRainTimer.UpdateActivates(deltaTime))
            {
                var pickup = _PoolManager.PickupPool.Get<WeaponPickup>();
                pickup.FireStrategyIndex = GetRandomAssignableFireIndex();

                pickup.transform.position = RandomRainPosition(pickup);
            }
        }

        private void OneUpRain(float deltaTime)
        {
            if (OneUpRainTimer.UpdateActivates(deltaTime))
            {
                var oneUp = _PoolManager.PickupPool.Get<OneUpPickup>();
                oneUp.transform.position = RandomRainPosition(oneUp);
            }
        }

        private void LateUpdate()
        {
            SyncTimeScales();
        }

        private void SyncTimeScales()
        {

        }

#endregion Update


#region Player Weapons

        private const int FireStrategyIndexBasic = 0;

        public float InitialWeaponTime => _InitialWeaponTime;
        public float WeaponRainTime => _WeaponRainTime;
        public float OneUpRainTime => _OneUpRainTime;
        public int WeaponLevel { get; set; }
        public float WeaponTimeMax { get; set; } = 10f;
        public FrameTimer WeaponResetTimer { get; set; }
        public LoopingFrameTimer WeaponRainTimer { get; set; }
        public LoopingFrameTimer OneUpRainTimer { get; set; }

        private PlayerFireStrategy CurrentFireStrategy => FireStrategies[FireStrategies.Index];
        private CircularSelector<PlayerFireStrategy> FireStrategies { get; set; }

        public int GetRandomAssignableFireIndex()
        {
            // Skip Basic fire, located at index 0
            const int FirstAssignable = FireStrategyIndexBasic + 1;
            int LastAssignableExclusive = FireStrategies.Count;
            int ret = RandomUtil.Int(FirstAssignable, LastAssignableExclusive);
            return ret;
        }

        public void SetFireType(int index, bool skipDropDown = false, bool skipMessage = false, bool endlessTime = false)
        {
            // Immediately fire BFG if it's charging
            if (BfgBulletSpawner.Instance.isActiveAndEnabled)
                FireCurrentStrategy();

            FireStrategies.Index = index;
            CurrentFireStrategy.Reset();
            CurrentFireStrategy.OnActivate();

            if (!skipDropDown)
                DebugUi.SetDropdownFiretype(FireStrategies.Index, true, endlessTime);

            WeaponResetTimer.Reset();
            if (index != DefaultFireTypeIndex)
            {
                if (endlessTime)
                    WeaponResetTimer.ActivationInterval = float.MaxValue;
                else
                    WeaponResetTimer.ActivationInterval = InitialWeaponTime;
            }
            else
            {
                WeaponResetTimer.ActivationInterval = InitialWeaponTime;
            }

            if (!skipMessage)
                NotificationManager.AddNotification(CurrentFireStrategy.NotificationName(WeaponLevel));

            if(endlessTime)
                SaveUtil.LastWeapon = index;
        }

        public void FirePlayerBullets(PlayerBullet[] bullets)
        {
            _PowerupManager.OnFire(Player.FirePosition, bullets);

        }
        public void FirePlayerBullet(PlayerBullet bullet)
        {
            FirePlayerBullets(new PlayerBullet[] { bullet });
        }

        public void IncreaseWeaponLevel()
        {
            if (WeaponLevel < GameConstants.MaxWeaponLevel)
            {
                DebugUi.SliderFireLevel.value = WeaponLevel + 1;

                string excitement = new string('!', WeaponLevel);
                string text = $"Weapon level up!\r\n{WeaponLevel}/{GameConstants.MaxWeaponLevel}{excitement}";
                NotificationManager.AddNotification(text);
            }
        }

#endregion Player Weapons

#region Powerups

        public PowerupManager _PowerupManager { get; } = new PowerupManager();

        public float PlayerFireDeltaTimeScale
        {
            get => _playerFireDeltaTimeScale;
            set
            {
                _playerFireDeltaTimeScale = value;
                CurrentFireStrategy.Reset();
                TimeScaleManager.Player = value;
            }
        }

        // Whether or not the current Victim was selected automatically
        // instead of being clicked by the player.
        public bool VictimWasAutomatic { get; set; }

        public Enemy VictimEnemy
        {
            get => _victimEnemy;
            set
            {
                // Don't handle logic if we're assigning the same victim
                if (_victimEnemy == value)
                    return;

                // Deactivate current marker
                if (_victimEnemy != null)
                    _victimEnemy.VictimMarker.StartDeactivation();

                _victimEnemy = value;

                var newMarker = _PoolManager.UIElementPool.Get<VictimMarker>();
                if (value != null)
                {
                    _victimEnemy.VictimMarker = newMarker;

                    if (Player.VictimMarker != null)
                    {
                        Player.VictimMarker.StartDeactivation();
                        Player.VictimMarker = null;
                    }
                }
                else
                {
                    if (Player.IsAlive)
                    {
                        Player.VictimMarker = newMarker;
                    }
                    else
                    {
                        newMarker.DeactivateSelf();
                    }
                }
            }
        }

        public Enemy MetronomeEnemy
        {
            get => _metronomeEnemy;
            set
            {
                // Don't handle logic if we're assigning the same metronome
                if (_metronomeEnemy == value)
                    return;

                // Deactivate label
                if (_metronomeEnemy != null)
                    _metronomeEnemy.MetronomeLabel.StartDeactivation();

                _metronomeEnemy = value;

                var newLabel = _PoolManager.UIElementPool.Get<MetronomeLabel>();
                _metronomeEnemy.MetronomeLabel = newLabel;
            }
        }

        public void OnEnemyHit(Enemy enemy, PlayerBullet bullet)
        {
            _PowerupManager.OnHit(enemy, bullet);
        }

#region OnEnemyKill

        public void OnEnemyKill(Enemy enemy, PlayerBullet bullet)
        {
            _PowerupManager.OnKill(enemy, bullet);
        }

        public void SetBloodlust(float duration, float speedScale)
        {
            Player.SetBloodlust(duration, speedScale);
            CurrentFireStrategy.Reset();
            _Othello.ResetFiretimer();
        }

#endregion OnEnemyKill

#endregion Powerups

#region Enemies

#region Enemy Bullets

        public void ReflectBullet(EnemyBullet target)
        {
            if (target.isActiveAndEnabled && target.CanReflect)
            {
                var reflectedBullet = _PoolManager.BulletPool.Get<ReflectedBullet>();
                reflectedBullet.ReflectBack(target);
                target.DeactivateSelf();
            }
        }

        public void ReflectBulletFromPestControl(EnemyBullet target, PestControlBullet pestControl)
        {
            if (target.isActiveAndEnabled && target.CanReflect)
            {
                var reflectedBullet = _PoolManager.BulletPool.Get<ReflectedBullet>();
                reflectedBullet.RedirectFromPestControl(target, pestControl);
                target.DeactivateSelf();
            }
        }

#endregion Enemy Bullets

#endregion Enemies

#region Damage

        public int LivesLeft
        {
            get => _RemainingLivesBar.SpritesToRender;
            set => _RemainingLivesBar.SpritesToRender = value;
        }

        public void OnGetHit()
        {
            _PowerupManager.OnGetHit();

            TakeDamage();
        }

        public void TakeDamage()
        {
            Director.ResetDifficulty();

            if (LivesLeft > 0)
            {
                //Player.CreateFleetingTextAtCenter("Ow");
                LivesLeft--;
            }
            else
            {
                int score = _Scoreboard.Score;
                //Player.CreateFleetingTextAtCenter("You died!!");
                //NotificationManager.AddNotification("You died!!");

                //bool newHighScore = score > SaveUtil.HighScore;
                //if (newHighScore)
                //{
                //    NotificationManager.AddNotification("New high score!");
                //    NotificationManager.AddNotification($"Old high score: {SaveUtil.HighScore}");
                //    NotificationManager.AddNotification($"Your score: {score}");
                //}
                //else
                //{
                //    NotificationManager.AddNotification($"Score: {score}");
                //    NotificationManager.AddNotification($"High score: {SaveUtil.HighScore}");
                //}

                SaveUtil.HighScore = _Scoreboard.Score;

                GameOver();

                //LivesLeft = _StartingExtraLives;

                //_Scoreboard.ResetScore();
            }
        }

        public bool CanGameOver { get; set; }
        private void GameOver()
        {
            if (CanGameOver)
            {
                _GameOverGUI.Activate(_Scoreboard.Score, SaveUtil.HighScore);

                Player.Kill();
                _Othello.Kill();
                _Monsoon.Kill();
                _MonsoonSpawner.Kill();
                _SentinelManager.Kill();

                _Scoreboard.gameObject.SetActive(false);
            }
            else
            {
                //NotificationManager.AddNotification("Game over skipped!");
            }
        }

#endregion Damage

#region Game Tasks

        private GameTaskListManager ManagedGameTasks { get; } = new GameTaskListManager();

        public void StartTask(GameTask task)
        {
            ManagedGameTasks.AddTask(task);
        }

        public void GameTaskRunnerDeactivated(ValkyrieSprite target)
        {
            ManagedGameTasks.GameTaskRunnerDeactivated(target);
        }

#endregion Game Tasks

#region Powerup Menu

        public void AddPowerupMenuTitleRow(string title)
        {
            _PowerupMenu.AddTitleRow(title);
        }
        public void AddPowerupMenuPowerupRow(Powerup powerup)
        {
            _PowerupMenu.AddPowerupRow(powerup);
        }

        public void TogglePowerupMenuVisibility()
        {
            bool visible = _PowerupMenu.gameObject.activeSelf;
            _PowerupMenu.gameObject.SetActive(!visible);
        }

        public void PowerupRowPowerLevelChanged(int value)
        {
            _PowerupMenu.SetLevel(SaveUtil.LastPowerup.GetType(), value);
        }
#endregion Powerup Menu

#region Debug

        public void DebugIncrementFireType()
        {
            SetFireType(FireStrategies.Index + 1, endlessTime: true);
        }
        public void DebugDecrementFireType()
        {
            SetFireType(FireStrategies.Index - 1, endlessTime: true);
        }

        public void RecolorPlayerActivity(Color color)
        {
            _ColorManager.DefaultPlayer = color;
            Player.GetComponent<SpriteRenderer>().color = _ColorManager.DefaultPlayer;

            _PoolManager.RecolorPlayerActivity(color);
        }

#endregion Debug

        /// <summary>
        /// Creates a Fleeting Text with a specified message at a specified position.
        /// </summary>
        /// <param name="message">The message to give the fleeting text.</param>
        /// <param name="position">The position to place the fleeting text.</param>
        /// <returns></returns>
        public FleetingText CreateFleetingText(string message, Vector3 position)
        {
            var text = _PoolManager.UIElementPool.Get<FleetingText>(position);
            text.Text = message;

            return text;
        }

        public static void ResetScene()
        {
            var scene = SceneManager.GetActiveScene();
            int index = scene.buildIndex;
            SceneManager.LoadScene(index);
        }
    }
}
