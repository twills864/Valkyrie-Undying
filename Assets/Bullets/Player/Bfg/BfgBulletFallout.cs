using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.ColorManagers;
using Assets.Constants;
using Assets.FireStrategies.PlayerFireStrategies;
using Assets.GameTasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    public class BfgBulletFallout : PlayerBullet
    {
        private static BfgBulletFallout Instance { get; set; }

        public static void ActivateInstance()
        {
            if(!Instance.isActiveAndEnabled)
                Instance.ActivateSelf();
            else
                Instance.FadeOutSequence.ResetSelf();
        }

        [SerializeField]
        private float MaxAlpha = GameConstants.PrefabNumber;

        private Color FullBright;
        private float DelayTime;
        private float FadeTime;
        private SequenceGameTask FadeOutSequence { get; set; } = SequenceGameTask.Default();

        // Call from BulletPoolList -> BfgBulletPrefab.InitFallout()
        public static void StaticInitFadeInfo(float delayTime, float fadeTime)
        {
            Instance = PoolManager.Instance.BulletPool.Get<BfgBulletFallout>();
            Instance.DeactivateSelf();

            Instance.DelayTime = delayTime;
            Instance.FadeTime = fadeTime;
        }

        // Call from GameManager.InitWithDependencies()
        public static void StaticInitColors(in ColorManager colorManager)
        {
            Instance.FullBright = colorManager.DefaultPlayer;
            Instance.OnStaticInit();
        }

        private void OnStaticInit()
        {
            FullBright.a = MaxAlpha;

            var worldSize = SpaceUtil.WorldMapSize;
            var spriteSize = Instance.Sprite.size;

            float scaleX = worldSize.x / spriteSize.x;
            float scaleY = worldSize.y / spriteSize.y;
            float scaleZ = 1.0f;
            transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            transform.position = Vector3.zero;

            Alpha = FullBright.a;

            var delay = new Delay(this, DelayTime);
            var fadeOut = new FadeTo(this, 0, FadeTime);
            var deactivate = new GameTaskFunc(this, DeactivateSelf);
            FadeOutSequence = new SequenceGameTask(this, delay, fadeOut, deactivate);
        }

        protected override void OnPlayerBulletInit()
        {
            Instance = this;
        }

        protected override void OnActivate()
        {
            transform.position = Vector3.zero;
            Alpha = FullBright.a;
            FadeOutSequence.ResetSelf();
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime)
        {
            FadeOutSequence.RunFrame(deltaTime);
        }
    }
}
