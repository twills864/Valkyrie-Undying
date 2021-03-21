using Assets.Bullets.EnemyBullets;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.EnemyBullets
{
    /// <inheritdoc/>
    public class LaserEnemyBullet : EnemyBullet
    {
        private const float FullBright = 1.0f;

        public override bool DeactivateOnHit => false;

        //[SerializeField]
        //private float ScaleY = GameConstants.PrefabNumber;

        [SerializeField]
        private float FullBrightTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float FadeTime = GameConstants.PrefabNumber;

        private Sequence Sequence { get; set; }

        public override int ReflectedDamage => 0;
        public override bool CanReflect => false;
        protected override bool ShouldDeactivateOnDestructor => false;

        public bool HitBoxActive { get; private set; }

        public float WidthHalf { get; private set; }
        public bool SequenceActive { get; set; }

        protected override void OnEnemyBulletInit()
        {
            var sprite = GetComponent<SpriteRenderer>();
            WidthHalf = sprite.size.x * 0.5f * transform.localScale.x;

            //LocalScaleY = ScaleY;

            //float worldHeight = SpaceUtil.WorldMap.Height;

            //var spriteHeight = Sprite.size.x;
            //var heightScale = worldHeight / spriteHeight;
            //transform.localScale = new Vector3(1, heightScale, 1);

            var delay = new Delay(this, FullBrightTime);
            var removeActive = new GameTaskFunc(this, () => HitBoxActive = false);
            var fadeTo = new FadeTo(this, 0.0f, FadeTime);
            var deactivate = GameTaskFunc.DeactivateSelf(this);

            Sequence = new Sequence(delay, removeActive, fadeTo, deactivate);
        }

        protected override void OnActivate()
        {
            HitBoxActive = true;
            Alpha = FullBright;
            Sequence.ResetSelf();
            SequenceActive = false;
        }

        protected override void OnFrameRun(float deltaTime)
        {
            if (SequenceActive)
                Sequence.RunFrame(deltaTime);
        }

        //protected override void OnPlayerBulletTriggerEnter2D(Collider2D collision)
        //{
        //    if (IsMaxLevel)
        //    {
        //        if (CollisionUtil.IsEnemyBullet(collision))
        //        {
        //            var bullet = collision.GetComponent<EnemyBullet>();
        //            bullet.DeactivateSelf();
        //        }
        //    }
        //}
    }
}