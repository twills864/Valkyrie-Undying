using Assets.Bullets;
using Assets.Constants;
using Assets.Enemies;
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
        public Player Player;
        public DebugUI DebugUi;
        public Color PlayerColor;
        public Canvas _Canvas;

        private Camera _Camera;

        [SerializeField]
        private BasicBullet _BasicBullet;
        [SerializeField]
        private FleetingText _FleetingText;
        [SerializeField]
        private Destructor _Destructor;
        [SerializeField]
        private Enemy _DebugEnemy;

        [SerializeField]
        private Component BulletManager;

        private TrackedObjectList<Bullet> Bullets = new TrackedObjectList<Bullet>();

        void Start()
        {
            //Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            Init();

            _Camera = Camera.main;
            _Canvas = FindObjectOfType<Canvas>();

            PlayerColor = InitPlayerColor();
            Player.GetComponent<SpriteRenderer>().color = PlayerColor;
            Player.Init();
        }

        public void SpawnBasicBullet()
        {
            //var bullet = Instantiate(_BasicBullet);
            var pos = Player.FirePosition();

            var bullet = PoolManager.Get<BasicBullet>();
            bullet.Init(pos);
            //bullet.transform.position = Player.FirePosition();

            var basic = BulletManager.transform.Find("Basic");
            bullet.transform.SetParent(basic);

            Bullets.Add(bullet);
        }

        private void Init()
        {
            SpaceUtil.Init();
            DebugUi.Init(this);
            DebugUtil.Init(DebugUi, this);
            RandomUtil.Init();

            _Destructor.Init();
            _DebugEnemy.Init();

            InitPoolManager();
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
            RemoveInactiveElements();
            PoolManager.DebugInfo();

            DebugUtil.HandleInput();

            float deltaTime = Time.deltaTime;

#if DEBUG
            var distinctBullets = Bullets.Distinct().ToList();
            if(distinctBullets.Count != Bullets.Count)
            {

            }
#endif

            Bullets.RemoveInactiveElements();
            for (int i = 0; i < Bullets.Count; i++)
            {
                var bullet = Bullets[i];
                bullet.RunFrame(deltaTime);
            }
        }
        private void RemoveInactiveElements()
        {
            Bullets.RemoveInactiveElements();
        }

        private void OnGUI()
        {
        }

        private void Awake()
        {
            SetFrameRate();
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

            foreach(var bullet in Bullets)
            {
                bullet.GetComponent<SpriteRenderer>().color = PlayerColor;
            }
        }

        public FleetingText CreateFleetingText(string text, Vector2 worldPosition)
        {
            var newFleetingText = Instantiate(_FleetingText);
            newFleetingText.transform.SetParent(_Canvas.transform, true);

            var position = Camera.main.WorldToScreenPoint(worldPosition);

            newFleetingText.Init(text, position);

            return newFleetingText;
        }



        // This class was created for ease and speed of development.
        // It should be replaced with a hard-coded version upon release.
        public void InitPoolManager()
        {
            var privatePrefabs = ReflectionUtil.GetPrivatePoolablePrefabFields(this);
            PoolManager.InitPool(privatePrefabs);
        }
    }
}