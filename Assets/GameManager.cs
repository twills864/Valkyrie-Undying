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

namespace Assets
{
    /// <inheritdoc/>
    public class GameManager : MonoBehaviour
    {
        #region Debug

        private TestingType CurrentTest = TestingType.NewPowerup;

        public static Type OverrideEnemyType => null; // DebugUtil.GetOverrideEnemyType<BasicEnemy>();

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

        public int DefaultFireTypeIndex => SaveUtil.LastWeapon;

        #endregion Debug


        #region Property Fields

        private Enemy _victimEnemy;
        private Enemy _metronomeEnemy;
        private float _playerFireDeltaTimeScale = 1f;

        #endregion Property Fields


        public static GameManager Instance { get; private set; }


        #region Prefabs

        [SerializeField]
        private PlayerFireStrategyManager _FireStrategyManager;

        [SerializeField]
        private PowerupBalanceManager PowerupBalance;

        [SerializeField]
        private DirectorBalance _DirectorBalance;

        [SerializeField]
        private ColorManager _ColorManager;

        #region Player Prefabs

        [SerializeField]
        public Player Player;

        [SerializeField]
        public Othello _Othello;

        [SerializeField]
        public int _StartingExtraLives = GameConstants.PrefabNumber;

        [SerializeField]
        private Scoreboard _Scoreboard = null;

        #endregion Player Prefabs

        #region Powerup Prefabs

        [SerializeField]
        private PowerupMenu _PowerupMenu = null;

        [SerializeField]
        private Monsoon _Monsoon = null;

        [SerializeField]
        private MonsoonSpawner _MonsoonSpawner = null;

        [SerializeField]
        private SentinelManager _SentinelManager = null;

        #endregion Powerup Prefabs

        #region UI Prefabs

        [SerializeField]
        private RepeatingSpriteBar _RemainingLivesBar = null;

        #endregion UI Prefabs

        #region Debug Prefabs

        [SerializeField]
        public Enemy _DebugEnemy;
        [SerializeField]
        public DebugUI DebugUi;

        #endregion Debug Prefabs

        #region Screen Prefabs

        [SerializeField]
        private Destructor _Destructor = null;
        [SerializeField]
        private ScreenEdgeColliderSet _ScreenEdgeColliderSet = null;

        #endregion Screen Prefabs

        #region Misc Prefabs

        [SerializeField]
        private PoolManager _PoolManager = null;

        [SerializeField]
        private float _InitialWeaponTime = GameConstants.PrefabNumber;

        [SerializeField]
        private Sprite _LifeSprite = null;

        #endregion Misc Prefabs

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
            SpaceUtil.Init();
            RandomUtil.Init();

            // _ColorManager is a prefab field, and doesn't need initialized.
            _PoolManager.Init(in _ColorManager);

            WeaponResetTimer = new FrameTimer(InitialWeaponTime);

            _PowerupMenu.Init();
            _PowerupMenu.transform.position += new Vector3(0, 0, 0);

            MonsoonSpawner.Instance = _MonsoonSpawner;

            _Scoreboard.Init();
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

            _RemainingLivesBar.Init();

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
            Player.SpriteColor = _ColorManager.DefaultPlayer;
            _Othello.SpriteColor = _ColorManager.DefaultPlayerColorWithAlpha(_ColorManager.Player.OthelloAlpha);
            _Monsoon.SpriteColor = defaultPlayerAdditional;
            _MonsoonSpawner.SpriteColor = defaultPlayerAdditional;
        }

        private void InitGame()
        {
            LivesLeft = _StartingExtraLives;
        }

        #endregion Init

        #region Update

        private void Update()
        {
            //var lol = Instantiate(_LifeSprite);

            if (DebugPauseNextFrame)
            {
                Debugger.Break();
                UnityEngine.Debug.Break();
                DebugPauseNextFrame = false;
            }

            DebugUtil.HandleInput();

            float deltaTime = Time.deltaTime;
            float playerTime = deltaTime;
            float playerFireScale = deltaTime * PlayerFireDeltaTimeScale;

            if (CurrentFireStrategy.UpdateActivates(playerFireScale * Player.FireSpeedScale))
            {
                var bullets = CurrentFireStrategy.GetBullets(WeaponLevel, Player.FirePosition);
                FirePlayerBullets(bullets);

                _Othello.Fire();
            }

            UpdateFireStrategy(playerTime);

            float enemyTimeScale = TimeScaleManager.GetTimeScaleModifier(TimeScaleType.Enemy);
            float enemyDeltaTime = deltaTime * enemyTimeScale;
            Director.RunFrame(enemyDeltaTime, deltaTime);

            _PowerupManager.PassiveUpdate(playerFireScale, deltaTime);
            GameTaskLists.RunFrames(playerFireScale, deltaTime, deltaTime, deltaTime);
        }

        private void UpdateFireStrategy(float playerTime)
        {
            if (FireStrategies.Index != FireStrategyIndexBasic)
            {
                if (WeaponResetTimer.UpdateActivates(playerTime))
                    SetFireType(FireStrategyIndexBasic, skipMessage: true);
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
        public int WeaponLevel { get; set; }
        public float WeaponTimeMax { get; set; } = 10f;
        public FrameTimer WeaponResetTimer { get; set; }

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
            BfgBulletSpawner.Instance.DeactivateSelf();

            FireStrategies.Index = index;
            CurrentFireStrategy.Reset();
            CurrentFireStrategy.OnActivate();

            if (!skipDropDown)
                DebugUi.SetDropdownFiretype(FireStrategies.Index, true, endlessTime);

            WeaponResetTimer.Reset();
            if (index != DefaultFireTypeIndex && endlessTime)
                WeaponResetTimer.ActivationInterval = float.MaxValue;
            else
                WeaponResetTimer.ActivationInterval = InitialWeaponTime;


            if(!skipMessage)
                CreateFleetingText(CurrentFireStrategy.StrategyName, SpaceUtil.WorldMap.Center);

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

                string text = $"Weapon level up! {WeaponLevel}/{GameConstants.MaxWeaponLevel}";
                CreateFleetingText(text, Player.FirePosition);
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

                    if(Player.VictimMarker != null)
                    {
                        Player.VictimMarker.StartDeactivation();
                        Player.VictimMarker = null;
                    }
                }
                else
                    Player.VictimMarker = newMarker;
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

        public PowerupPickup SpawnPowerup(Vector3 position)
        {
            var pickup = _PoolManager.PickupPool.GetRandomPowerup(position);
            return pickup;
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
            if (LivesLeft > 0)
            {
                //Player.CreateFleetingTextAtCenter("Ow");
                LivesLeft--;
            }
            else
            {
                Player.CreateFleetingTextAtCenter("You died!!");
                LivesLeft = _StartingExtraLives;
                _Scoreboard.ResetScore();
            }



        }

        #endregion Damage

        #region Game Tasks

        private GameTaskListManager GameTaskLists = new GameTaskListManager();

        public void StartTask(GameTask task, TimeScaleType taskType)
        {
            GameTaskLists.StartTask(task, taskType);
        }

        public void GameTaskRunnerDeactivated(ValkyrieSprite target)
        {
            GameTaskLists.GameTaskRunnerDeactivated(target);
        }


        public void AddPlayerTask(GameTask task)
        {
            GameTaskLists.PlayerGameTaskList.Add(task);
        }
        public void AddBulletTask(GameTask task)
        {
            GameTaskLists.BulletGameTaskList.Add(task);
        }
        public void AddEnemyTask(GameTask task)
        {
            GameTaskLists.EnemyGameTaskList.Add(task);
        }
        public void AddEnemyBulletTask(GameTask task)
        {
            GameTaskLists.EnemyBulletGameTaskList.Add(task);
        }
        public void AddUIElementTask(GameTask task)
        {
            GameTaskLists.UIElementGameTaskList.Add(task);
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
    }
}
