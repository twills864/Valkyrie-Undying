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

        [SerializeField]
        private float FadeInTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float MaxAlpha = GameConstants.PrefabNumber;

        [SerializeField]
        private float FallbackDeactivationTime = GameConstants.PrefabNumber;

        private float InitialScaleX;
        private float ScaleXPerLevel;
        private ScaleTo ScaleIn { get; set; }
        private SequenceGameTask FallbackDeactivate { get; set; }


        protected override void OnPlayerBulletInit()
        {
            float worldHeight = SpaceUtil.WorldMap.Height;

            float spriteHeight = Sprite.size.y;
            float heightScale = worldHeight / spriteHeight;
            transform.localScale = new Vector3(0f, heightScale, 1f);

            Alpha = 0f;

            Vector3 scale = new Vector3(1f, heightScale, 1f);
            ScaleIn = new ScaleTo(this, scale, FadeInTime);
            var fadeIn = new FadeTo(this, MaxAlpha, float.Epsilon); //  FadeInTime * 0.25f
            var concurrence = new ConcurrentGameTask(this, ScaleIn, fadeIn);

            var delay = new Delay(this, FallbackDeactivationTime - FadeInTime);
            var deactivate = new GameTaskFunc(this, DeactivateSelf);
            FallbackDeactivate = new SequenceGameTask(this, concurrence, delay, deactivate);
        }

        protected override void OnActivate()
        {
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

        protected override void OnPlayerBulletFrameRun(float deltaTime)
        {
            FallbackDeactivate.RunFrame(deltaTime);
            PositionX = Player.Instance.transform.position.x;
        }
    }
}
