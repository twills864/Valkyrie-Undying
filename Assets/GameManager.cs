using Assets.Bullet;
using Assets.Util;
using Assets.Util.AssetsDebug;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            var bullet = Instantiate(_BasicBullet);
            bullet.transform.position = Player.FirePosition();
        }

        private void Init()
        {
            SpaceUtil.Init();
            DebugUi.Init(this);
            DebugUtil.Init(DebugUi, this);
            RandomUtil.Init();

            _Destructor.Init();
            _DebugEnemy.Init();
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
            DebugUtil.HandleInput();
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
        }

        public FleetingText CreateFleetingText(string text, Vector2 worldPosition)
        {
            var newFleetingText = Instantiate(_FleetingText);
            newFleetingText.transform.SetParent(_Canvas.transform, true);

            var position = Camera.main.WorldToScreenPoint(worldPosition);

            newFleetingText.Init(text, position);

            return newFleetingText;
        }
    }
}