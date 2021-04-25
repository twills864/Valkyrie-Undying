using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.FireStrategies.PlayerFireStrategies;
using Assets.GameTasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    public class BfgBulletSpawner : PlayerBullet
    {
        public static BfgBulletSpawner Instance { get; set; }

        public static bool TryGetInactiveSpawner(out BfgBulletSpawner bullet)
        {
            bullet = Instance;
            return !bullet.isActiveAndEnabled;
        }

        public static void StaticInit()
        {
            Instance = PoolManager.Instance.BulletPool.Get<BfgBulletSpawner>();
            Instance.DeactivateSelf();
        }

        public static void StaticInitScale(float initialScaleX, float scaleXPerLevel)
        {
            Instance.InitialScaleX = initialScaleX;
            Instance.ScaleXPerLevel = scaleXPerLevel;
        }

        #region Property Fields

        private float _fallbackDeactivationTime;

        #endregion Property Fields


        #region Prefabs

        [SerializeField]
        private float _FadeInTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _MaxAlpha = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private float FadeInTime => _FadeInTime;

        private float MaxAlpha => _MaxAlpha;

        #endregion Prefab Properties


        private float InitialScaleX { get; set; }
        private float ScaleXPerLevel { get; set; }
        private ScaleTo ScaleIn { get; set; }
        private Delay DeactivateDelay { get; set; }
        private Sequence FallbackDeactivate { get; set; }

        public float FallbackDeactivationTime
        {
            get => _fallbackDeactivationTime;
            set
            {
                const float TimeDelta = 5f / 60f;

                if (_fallbackDeactivationTime != value)
                {
                    _fallbackDeactivationTime = value;
                    DeactivateDelay.Duration = (value - FadeInTime) + TimeDelta;
                    FallbackDeactivate.RecalculateDuration();
                }
            }
        }

        protected override void OnPlayerBulletInit()
        {
            float worldHeight = SpaceUtil.WorldMap.Height;

            float spriteHeight = Sprite.size.y;
            float heightScale = worldHeight / spriteHeight;
            transform.localScale = new Vector3(0f, heightScale, 1f);

            Alpha = 0f;

            Vector3 scaleInitial = new Vector3(0f, heightScale, 1f);
            Vector3 scaleFinal = new Vector3(1f, heightScale, 1f);
            ScaleIn = new ScaleTo(this, scaleInitial, scaleFinal, FadeInTime);
            var fadeIn = new FadeTo(this, MaxAlpha, float.Epsilon); //  FadeInTime * 0.25f
            var concurrence = new ConcurrentGameTask(this, ScaleIn, fadeIn);

            DeactivateDelay = new Delay(this, FallbackDeactivationTime - FadeInTime);
            var deactivate = new GameTaskFunc(this, DeactivateSelf);
            FallbackDeactivate = new Sequence(concurrence, DeactivateDelay, deactivate);
        }

        protected override void OnActivate()
        {
            LocalScaleX = 0f;
            FallbackDeactivate.ResetSelf();
        }

        public override void OnSpawn()
        {
            float y = transform.position.y + SpaceUtil.WorldMap.HeightHalf;
            PositionY = y;


            var endValue = ScaleIn.EndValue;
            float x = InitialScaleX + (BulletLevel * ScaleXPerLevel);
            y = endValue.y;
            float z = endValue.z;
            ScaleIn.EndValue = new Vector3(x, y, z);
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            FallbackDeactivate.RunFrame(deltaTime);
            PositionX = Player.Instance.transform.position.x;
        }
    }
}
