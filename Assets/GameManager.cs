﻿using System;
using Assets.Bullets;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.FireStrategies.PlayerFireStrategies;
using Assets.GameTasks;
using Assets.GameTasks.GameTaskLists;
using Assets.ScreenEdgeColliders;
using Assets.UI;
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
        private Enemy _DebugEnemy;

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

            FireStrategies = new CircularSelector<PlayerFireStrategy>
            {
                new BasicStrategy(),
                new ShotgunStrategy(_PoolManager.BulletPool.GetPrefab<ShotgunBullet>()),
                new BurstStrategy(_PoolManager.BulletPool.GetPrefab<BurstBullet>()),
                new BounceStrategy(_PoolManager.BulletPool.GetPrefab<BounceBullet>()),
                new AtomStrategy(_PoolManager.BulletPool.GetPrefab<AtomBullet>()),
            };
            FireTimer = CurrentFireStrategy.FireTimer;

            Init();

            _Camera = Camera.main;
            _Canvas = FindObjectOfType<Canvas>();

            PlayerColor = InitPlayerColor();
            Player.GetComponent<SpriteRenderer>().color = PlayerColor;
            Player.Init();
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

            // GameTask goal - make this line execute
            var moveTo = new MoveTo(_DebugEnemy, SpaceUtil.WorldMap.Center,
                _DebugEnemy.transform.position, TimeConstants.OneSecond);
            _DebugEnemy.StartTask(moveTo);
            //_DebugEnemy.StartTask(MoveTo.Create(SpaceUtil.ScreenMap.Center, TimeConstants.OneSecond));

            EnemyTimer.ActivateSelf();
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

            if (FireTimer.UpdateActivates(deltaTime))
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
        public void DebugIncrementFireType()
        {
            SetFireType(FireStrategies.Index + 1);
        }
        public void DebugDecrementFireType()
        {
            SetFireType(FireStrategies.Index - 1);
        }

        private void OnGUI()
        {
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

        public AtomTrail GetAtomTrail()
        {
            var ret = _PoolManager.UIElementPool.Get<AtomTrail>();
            return ret;
        }

        #region Add Game Tasks

        public void StartTask(GameTask task, GameTaskType taskType)
        {
            switch (taskType)
            {
                case GameTaskType.Player:
                    GameManager.Instance.AddPlayerTask(task);
                    break;
                case GameTaskType.Bullet:
                    GameManager.Instance.AddBulletTask(task);
                    break;
                case GameTaskType.Enemy:
                    GameManager.Instance.AddEnemyTask(task);
                    break;
                case GameTaskType.EnemyBullet:
                    GameManager.Instance.AddEnemyBulletTask(task);
                    break;
                case GameTaskType.UIElement:
                    GameManager.Instance.AddUIElementTask(task);
                    break;
                default:
                    throw ExceptionUtil.ArgumentException(taskType);
            }
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

        public void RecolorPlayerActivity(Color color)
        {
            PlayerColor = color;
            Player.GetComponent<SpriteRenderer>().color = PlayerColor;

            _PoolManager.RecolorPlayerActivity(color);
        }

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