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
    public class RetributionBullet : VoidEffectBullet
    {
        public override int Damage => base.Damage * BulletLevel;

        #region Prefabs

        [SerializeField]
        private float _FadeTime = GameConstants.PrefabNumber;

        #endregion Prefabs

        #region Prefab Properties

        private float FadeTime => _FadeTime;

        #endregion Prefab Properties

        private float Scale { get; set; }
        private float Duration { get; set; }

        private EaseIn3 ScaleIn { get; set; }
        private FadeTo Fade { get; set; }

        private Sequence Sequence { get; set; }

        public static RetributionBullet StartRetribution(Vector3 position, int level, float scale, float duration)
        {
            var bullet = PoolManager.Instance.BulletPool.Get<RetributionBullet>(position);
            bullet.Init(level, scale, duration);
            bullet.OnSpawn();

            return bullet;
        }

        protected override void OnActivate()
        {
            Color color = Sprite.color;
            color.a = 1.0f;
            Sprite.color = color;
        }

        private void Init(int level, float scale, float duration)
        {
            BulletLevel = level;
            Scale = scale;
            Duration = duration;

            var scaleIn = new ScaleTo(this, InitialScale, Scale, Duration);
            ScaleIn = new EaseIn3(scaleIn);

            Fade = new FadeTo(this, 0, FadeTime);

            Sequence = new Sequence(ScaleIn, Fade);
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            if (!Sequence.IsFinished)
                Sequence.RunFrame(deltaTime);
            else
                DeactivateSelf();
        }
    }
}