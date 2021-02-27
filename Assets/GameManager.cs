﻿using System;
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

namespace Assets
{
    /// <inheritdoc/>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; set; }

        private const bool AddingPowerup = false;
        public Type GameRowPowerupType = typeof(VictimPowerup);

        public Player Player;
        public Othello _Othello;
        public DebugUI DebugUi;
        public Color PlayerColor;
        public Canvas _Canvas;

        private Camera _Camera;

        #region Prefabs

        [SerializeField]
        private BasicBullet _BasicBullet;
        [SerializeField]
        private FleetingText _FleetingText;
        [SerializeField]
        private Destructor _Destructor;
        [SerializeField]
        private ScreenEdgeColliderSet _ScreenEdgeColliderSet;
        [SerializeField]
        public Enemy _DebugEnemy;

        [SerializeField]
        private PoolManager _PoolManager;

        #endregion Prefabs

        #region Player Weapons

        public int WeaponLevel { get; set; }

        public int DefaultFireTypeIndex => AddingPowerup ? 0 : FireStrategies.Count - 1;
        private LoopingFrameTimer FireTimer;
        private PlayerFireStrategy CurrentFireStrategy => FireStrategies[FireStrategies.Index];
        private CircularSelector<PlayerFireStrategy> FireStrategies;

        #endregion Player Weapons

        #region Powerups

        public PowerupManager _PowerupManager { get; set; }

        private float _playerFireDeltaTimeScale = 1f;
        public float PlayerFireDeltaTimeScale
        {
            get => _playerFireDeltaTimeScale;
            set
            {
                _playerFireDeltaTimeScale = value;
                CurrentFireStrategy.Reset();
            }
        }

        [SerializeField]
        private RainCloud _RainCloud;
        [SerializeField]
        private RainCloudSpawner _RainCloudSpawner;
        private RainCloudPowerup _RainCloudPowerup;

        [SerializeField]
        private SentinelManager _SentinelManager;

        public Enemy VictimEnemy
        {
            get => _victimEnemy;
            set
            {
                if (_victimEnemy == value)
                    return;

                if(_victimEnemy != null)
                    _victimEnemy.VictimMarker.StartDeactivation();

                _victimEnemy = value;
                _victimEnemy.VictimMarker = _PoolManager.UIElementPool.Get<VictimMarker>();
            }
        }
        public bool TryGetVictim(out Enemy victim)
        {
            victim = VictimEnemy;
            return victim != null;
        }

        #endregion Powerups

        #region Powerup Menu

        [SerializeField]
        private PowerupMenu _PowerupMenu;

        public void AddPowerupMenuTitleRow(string title)
        {
            _PowerupMenu.AddTitleRow(title);
        }
        public void AddPowerupMenuPowerupRow(Powerup powerup)
        {
            _PowerupMenu.AddPowerupRow(powerup);
        }

        public void SetPowerupMenuVisibility(bool visible)
        {
            _PowerupMenu.gameObject.SetActive(visible);
        }

        public void PowerupRowPowerLevelChanged(int value)
        {
            _PowerupMenu.SetLevel(GameRowPowerupType, value);
        }

        public void PowerupMenuPowerLevelRowSet(Powerup powerup, int level)
        {
            DebugUi.PowerupMenuPowerLevelRowSet(powerup, level);
        }

        #endregion Powerup Menu

        private GameTaskListManager GameTaskLists = new GameTaskListManager();

        private LoopingFrameTimer EnemyTimer = new LoopingFrameTimer(3.0f);
        private Enemy _victimEnemy;

        private void Awake()
        {
            Instance = this;
            SetFrameRate();
        }
        private void Start()
        {
            _PoolManager.Init();

            _PowerupManager = new PowerupManager();
            _PowerupManager.Init();

            _RainCloudPowerup = _PowerupManager.GetOnLevelUpPowerup<RainCloudPowerup>();

            RainCloud.Instance = _RainCloud;
            RainCloudSpawner.Instance = _RainCloudSpawner;

            FireStrategies = new CircularSelector<PlayerFireStrategy>
            {
                new BasicStrategy(),
                new ShotgunStrategy(_PoolManager.BulletPool.GetPrefab<ShotgunBullet>()),
                new BurstStrategy(_PoolManager.BulletPool.GetPrefab<BurstBullet>()),
                new BounceStrategy(_PoolManager.BulletPool.GetPrefab<BounceBullet>()),
                new AtomStrategy(_PoolManager.BulletPool.GetPrefab<AtomBullet>()),
                new SpreadStrategy(_PoolManager.BulletPool.GetPrefab<SpreadBullet>()),
                new FlakStrategy(_PoolManager.BulletPool.GetPrefab<FlakBullet>()),
                new TrampolineStrategy(_PoolManager.BulletPool.GetPrefab<TrampolineBullet>()),
            };
            FireTimer = CurrentFireStrategy.FireTimer;

            Init();

            _Camera = Camera.main;
            _Canvas = FindObjectOfType<Canvas>();

            PlayerColor = InitPlayerColor();
            Player.GetComponent<SpriteRenderer>().color = PlayerColor;
            Player.Init();

            //GameTaskLists.SetDebugUi();
        }

        private void Init()
        {
            SpaceUtil.Init();
            DebugUi.Init(this.FireStrategies, _PowerupMenu);

            TimeSpan frameTime = TimeSpan.FromSeconds((double)1 / (double)Application.targetFrameRate);
            //DebugUI.SetDebugLabel("FRAMETIME", frameTime);
            SetFireType(DefaultFireTypeIndex);

            DebugUtil.Init(DebugUi, this);
            RandomUtil.Init();

            _Destructor.Init();
            _ScreenEdgeColliderSet.Init();
            _DebugEnemy.Init();
            _RainCloud.Init();
            _Othello.Init();
            _SentinelManager.Init();

            EnemyTimer.ActivateSelf();

            _PowerupMenu.transform.position += new Vector3(0, 80, 0);
            _PowerupMenu.gameObject.SetActive(AddingPowerup);
        }

        public void FirePlayerBullets(PlayerBullet[] bullets)
        {
            _PowerupManager.OnFire(Player.FirePosition(), bullets);

        }
        public void FirePlayerBullet(PlayerBullet bullet)
        {
            FirePlayerBullets(new PlayerBullet[] { bullet });
        }

        public void CreateShrapnel(Vector2 position)
        {
            var maxY = _Destructor.SizeHalf.y;

            if (position.y < maxY)
                _PoolManager.BulletPool.Get<ShrapnelBullet>(position);
        }

        public void CreateRaindrop(Vector2 position, int damage)
        {
            var raindrop = _PoolManager.BulletPool.Get<RaindropBullet>(position);
            raindrop.RaindropDamage = damage;
        }

        public void FirePestControl(Vector2 position, int numberToGet)
        {
            var targets = _PoolManager.EnemyBulletPool.GetPestControlTargets(numberToGet);

            numberToGet = targets.Length;
            var pestControls = _PoolManager.BulletPool.GetMany<PestControlBullet>(numberToGet);

            for (int i = 0; i < pestControls.Length; i++)
            {
                var pestControl = pestControls[i];
                var target = targets[i];

                pestControl.SetTarget(position, target);
            }
        }

        public void ReflectBullet(EnemyBullet target)
        {
            var reflectedBullet = _PoolManager.BulletPool.Get<ReflectedBullet>();
            reflectedBullet.ReflectBack(target);
            target.DeactivateSelf();
        }

        public void ReflectBulletFromPestControl(EnemyBullet target, PestControlBullet pestControl)
        {
            var reflectedBullet = _PoolManager.BulletPool.Get<ReflectedBullet>();
            reflectedBullet.RedirectFromPestControl(target, pestControl);
            target.DeactivateSelf();
        }


        private Color DefaultPlayerColor => new Color(96f / 255f, 211f / 255f, 255f / 255f);
        private Color InitPlayerColor()
        {
            // Override with random color
            Color ret = DefaultPlayerColor; // DebugUtil.GetRandomPlayerColor();

            return ret;
        }

        // Update is called once per frame
        void Update()
        {
            //PoolManager.DebugInfo();

            DebugUtil.HandleInput();

            if (Input.GetMouseButton(2))
                _DebugEnemy.transform.position = SpaceUtil.WorldPositionUnderMouse();

            float deltaTime = Time.deltaTime;
            float playerFireScale = deltaTime * PlayerFireDeltaTimeScale;

            Player.RunFrame(deltaTime);
            _Othello.RunFrame(playerFireScale);
            if (FireTimer.UpdateActivates(playerFireScale * Player.FireSpeedScale))
            {
                var bullets = CurrentFireStrategy.GetBullets(WeaponLevel, Player.FirePosition());
                FirePlayerBullets(bullets);

                _Othello.Fire();
            }

            if (EnemyTimer.UpdateActivates(deltaTime))
            {
                var enemy = _PoolManager.EnemyPool.SpawnRandomEnemy();
                enemy.Init(SpaceUtil.RandomEnemySpawnPosition(enemy));
            }

            if (_RainCloudSpawner.isActiveAndEnabled)
                _RainCloudSpawner.RunFrame(deltaTime);
            if (_RainCloud.isActiveAndEnabled)
                _RainCloud.RunFrame(deltaTime);

            _SentinelManager.transform.position = Player.transform.position;
            _SentinelManager.RunFrame(deltaTime);

            _PoolManager.RunPoolFrames(deltaTime, deltaTime);
            _PowerupManager.PassiveUpdate(deltaTime);
            GameTaskLists.RunFrames(deltaTime);
            _DebugEnemy.RunFrame(deltaTime);
        }

        public void SetFireType(int index, bool skipDropDown = false)
        {
            FireStrategies.Index = index;
            FireTimer = CurrentFireStrategy.FireTimer;
            FireTimer.ActivateSelf();

            if (!skipDropDown)
                DebugUi.DropdownFireType.value = FireStrategies.Index;
        }


        private void SetFrameRate()
        {
            // ONLY limit frame rate when playing via Unity editor
#if UNITY_EDITOR
            // VSync must be disabled to set custom framerate
            // 0 for no sync, 1 for panel refresh rate, 2 for 1/2 panel
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
#endif
        }

        #region OnEnemyHit

        public void OnEnemyHit(Enemy enemy, PlayerBullet bullet)
        {
            _PowerupManager.OnHit(enemy, bullet);
        }

        #endregion OnEnemyHit

        #region OnEnemyKill

        public void OnEnemyKill(Enemy enemy, PlayerBullet bullet)
        {
            _PowerupManager.OnKill(enemy, bullet);
        }

        public void SetBloodlust(float duration, float speedScale)
        {
            Player.SetBloodlust(duration, speedScale);
            FireTimer.ActivateSelf();
        }

        #endregion OnEnemyKill

        #region TryGetRandomEnemy

        public bool TryGetRandomEnemy(out Enemy enemy)
        {
            var ret = _PoolManager.EnemyPool.TryGetRandomObject(out enemy);
            return ret;
        }
        public bool TryGetRandomEnemyExcluding(Enemy exclusion, out Enemy enemy)
        {
            var ret = _PoolManager.EnemyPool.TryGetRandomObjectExcluding(exclusion, out enemy);
            return ret;
        }

        #endregion TryGetRandomEnemy

        public AtomTrail GetAtomTrail()
        {
            var ret = _PoolManager.UIElementPool.Get<AtomTrail>();
            return ret;
        }

        #region Add Game Tasks

        public void StartTask(GameTask task, GameTaskType taskType)
        {
            GameTaskLists.StartTask(task, taskType);
        }

        public void GameTaskRunnerDeactivated(GameTaskRunner target)
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

        #endregion Add Game Tasks

        #region Debug

        public void RecolorPlayerActivity(Color color)
        {
            PlayerColor = color;
            Player.GetComponent<SpriteRenderer>().color = PlayerColor;

            _PoolManager.RecolorPlayerActivity(color);
        }

        public void DebugIncrementFireType()
        {
            SetFireType(FireStrategies.Index + 1);
        }
        public void DebugDecrementFireType()
        {
            SetFireType(FireStrategies.Index - 1);
        }

        #endregion Debug

        /// <summary>
        /// Creates a Fleeting Text with a specified message at a specified position.
        /// </summary>
        /// <param name="message">The message to give the fleeting text.</param>
        /// <param name="position">The position to place the fleeting text.</param>
        /// <returns></returns>
        public FleetingText CreateFleetingText(string message, Vector2 position)
        {
            var text = _PoolManager.UIElementPool.Get<FleetingText>(position);
            text.Text = message;
            return text;
        }
    }
}