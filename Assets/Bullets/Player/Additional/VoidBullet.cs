using Assets.Bullets.EnemyBullets;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class VoidBullet : VoidEffectBullet
    {
        protected override bool ShouldReflectBullet => BulletLevel > 1;

        #region Prefabs

        [SerializeField]
        private float _ScaleTime = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private float ScaleTime => _ScaleTime;

        #endregion Prefab Properties


        private float Scale { get; set; }
        private float Duration { get; set; }

        private EaseIn3 ScaleIn { get; set; }
        private Delay Delay { get; set; }
        private EaseOut3 ScaleOut { get; set; }

        private Sequence Behavior { get; set; }

        public static VoidBullet StartVoid(Vector3 position, int level, float scale, float duration)
        {
            var bullet = PoolManager.Instance.BulletPool.Get<VoidBullet>(position);
            bullet.Init(level, scale, duration);
            bullet.OnSpawn();

            return bullet;
        }

        private void Init(int level, float scale, float duration)
        {
            BulletLevel = level;
            Scale = scale;
            Duration = duration;

            var scaleIn = new ScaleTo(this, InitialScale, Scale, ScaleTime);
            ScaleIn = new EaseIn3(scaleIn);
            Delay = new Delay(this, Duration);
            var scaleOut = new ScaleTo(this, Scale, InitialScale, ScaleTime);
            ScaleOut = new EaseOut3(scaleOut);

            Behavior = new Sequence(ScaleIn, Delay, ScaleOut);
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            if (!Behavior.IsFinished)
                Behavior.RunFrame(deltaTime);
            else
                DeactivateSelf();
        }
    }
}