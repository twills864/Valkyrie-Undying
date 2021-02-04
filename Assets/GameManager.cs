using Assets.Bullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.FireStrategies;
using Assets.Util;
using Assets.Util.AssetsDebug;
using Assets.Util.ObjectPooling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
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
        private Enemy _DebugEnemy;

        [SerializeField]
        private PoolManager _PoolManager;

        #endregion Prefabs

        public int DefaultFireTypeIndex => FireStrategies.Count - 1;
        private LoopingFrameTimer FireTimer;
        private FireStrategy CurrentFireStrategy => FireStrategies[FireStrategies.Index];
        private CircularSelector<FireStrategy> FireStrategies = new CircularSelector<FireStrategy>
        {
            new BasicStrategy(),
            new ShotgunStrategy(),
        };

        private LoopingFrameTimer EnemyTimer = new LoopingFrameTimer(3.0f);

        private void Awake()
        {
            Instance = this;
            SetFrameRate();
        }
        void Start()
        {
            DebugUI.SetDebugLabel("CurrentFireType", () => CurrentFireStrategy.GetType().Name);
            DebugUI.SetDebugLabel("FirePos", () => Player.FirePosition());
            //Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            Init();

            _Camera = Camera.main;
            _Canvas = FindObjectOfType<Canvas>();

            PlayerColor = InitPlayerColor();
            Player.GetComponent<SpriteRenderer>().color = PlayerColor;
            Player.Init();


            FireTimer = CurrentFireStrategy.DefaultFireTimer;
            SetFireType(DefaultFireTypeIndex);
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
            DebugUtil.Init(DebugUi, this);
            RandomUtil.Init();

            _Destructor.Init();
            _DebugEnemy.Init();

            _PoolManager.Init();
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
                var bullets = CurrentFireStrategy.GetBullets(Player.FirePosition());
                FirePlayerBullets(bullets);
            }

            if(EnemyTimer.UpdateActivates(deltaTime))
            {
                var enemy = _PoolManager.EnemyPool.GetRandomEnemy();
                enemy.Init(SpaceUtil.RandomEnemySpawnPosition(enemy));
            }



            _PoolManager.RunPoolFrames(deltaTime, deltaTime);
        }

        public void SetFireType(int index, bool skipDropDown = false)
        {
            FireStrategies.Index = index;
            FireTimer = CurrentFireStrategy.DefaultFireTimer;
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

        public void RecolorPlayerActivity(Color color)
        {
            PlayerColor = color;
            Player.GetComponent<SpriteRenderer>().color = PlayerColor;

            _PoolManager.RecolorPlayerActivity(color);
        }

        public FleetingText CreateFleetingText(string text, Vector2 worldPosition)
        {
            var newFleetingText = Instantiate(_FleetingText);
            newFleetingText.transform.SetParent(_Canvas.transform, true);

            var position = Camera.main.WorldToScreenPoint(worldPosition);

            newFleetingText.Init(text, position);

            return newFleetingText;
        }



        //// This class was created for ease and speed of development.
        //// It should be replaced with a hard-coded version upon release.
        //public void InitPoolManager()
        //{
        //    var privatePrefabs = ReflectionUtil.GetPrivatePoolablePrefabFields(this);
        //    PoolManager.InitPool(privatePrefabs);
        //}
    }
}