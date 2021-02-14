using System;
using Assets.Bullets;
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
using Assets.Util.ObjectPooling;
using UnityEngine;

namespace Assets
{
    /// <inheritdoc/>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; set; }

        public Player Player;
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

        public int DefaultFireTypeIndex => FireStrategies.Count - 1;
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


        #endregion Powerup Menu

        private GameTaskListManager GameTaskLists = new GameTaskListManager();

        private LoopingFrameTimer EnemyTimer = new LoopingFrameTimer(3.0f);

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

            FireStrategies = new CircularSelector<PlayerFireStrategy>
            {
                new BasicStrategy(),
                new ShotgunStrategy(_PoolManager.BulletPool.GetPrefab<ShotgunBullet>()),
                new BurstStrategy(_PoolManager.BulletPool.GetPrefab<BurstBullet>()),
                new BounceStrategy(_PoolManager.BulletPool.GetPrefab<BounceBullet>()),
                new AtomStrategy(_PoolManager.BulletPool.GetPrefab<AtomBullet>()),
                new SpreadStrategy(_PoolManager.BulletPool.GetPrefab<SpreadBullet>()),
                new FlakStrategy(_PoolManager.BulletPool.GetPrefab<FlakBullet>()),
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
            DebugUi.Init(this, this.FireStrategies);
            SetFireType(DefaultFireTypeIndex);

            DebugUtil.Init(DebugUi, this);
            RandomUtil.Init();

            _Destructor.Init();
            _ScreenEdgeColliderSet.Init();
            _DebugEnemy.Init();

            EnemyTimer.ActivateSelf();
        }

        public void FirePlayerBullets(Bullet[] bullets)
        {
            foreach (var bullet in bullets)
                FirePlayerBullet(bullet);
        }
        public void FirePlayerBullet(Bullet bullet)
        {
            // Does nothing yet; Will add OnFire() powerup functionality later
        }

        public void CreateShrapnel(Vector2 position)
        {
            var maxY = _Destructor.SizeHalf.y;

            if (position.y < maxY)
                _PoolManager.BulletPool.Get<ShrapnelBullet>(position);
        }


        private Color DefaultPlayerColor => new Color(96f / 255f, 211f / 255f, 255f / 255f);
        private Color InitPlayerColor()
        {
            // Override with random color
            Color ret = DebugUtil.GetRandomPlayerColor();

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

            if (FireTimer.UpdateActivates(deltaTime * PlayerFireDeltaTimeScale))
            {
                var bullets = CurrentFireStrategy.GetBullets(WeaponLevel, Player.FirePosition());
                FirePlayerBullets(bullets);
            }

            if(EnemyTimer.UpdateActivates(deltaTime))
            {
                var enemy = _PoolManager.EnemyPool.SpawnRandomEnemy();
                enemy.Init(SpaceUtil.RandomEnemySpawnPosition(enemy));
            }

            _PoolManager.RunPoolFrames(deltaTime, deltaTime);
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

        public void OnEnemyHit(Enemy enemy)
        {
            _PowerupManager.OnHit(enemy);
        }

        #endregion OnEnemyHit

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