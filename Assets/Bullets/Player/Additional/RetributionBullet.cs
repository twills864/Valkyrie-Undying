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
    public class RetributionBullet : PermanentVelocityPlayerBullet
    {
        protected sealed override bool ShouldDeactivateOnDestructor => false;
        protected sealed override bool ShouldMarkSelfCollision => false;

        #region Prefabs

        [SerializeField]
        private float _FadeTime = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private float FadeTime => _FadeTime;

        #endregion Prefab Properties

        public float RetributionTimescale => 1.0f - Alpha;

        private float Scale { get; set; }
        private float Duration { get; set; }

        private bool IsExploding { get; set; }

        private EaseIn3 ScaleIn { get; set; }
        private FadeTo Fade { get; set; }

        private Sequence Sequence { get; set; }

        private TrackedPooledObjectSet<Enemy> ManagedEnemies { get; set; }

        public static RetributionBullet StartRetribution(Vector3 position, int level, float duration)
        {
            var bullet = PoolManager.Instance.BulletPool.Get<RetributionBullet>(position);
            bullet.Init(level, duration);
            bullet.OnSpawn();

            return bullet;
        }

        protected override void OnPermanentVelocityBulletInit()
        {
            Scale = SpaceUtil.WorldMap.Height;
            ManagedEnemies = new TrackedPooledObjectSet<Enemy>();
        }

        protected override void OnActivate()
        {
            Color color = Sprite.color;
            color.a = 1.0f;
            Sprite.color = color;

            IsExploding = true;
            ManagedEnemies.Clear();
        }

        private void Init(int level, float duration)
        {
            BulletLevel = level;
            Duration = duration;

            var scaleIn = new ScaleTo(this, float.Epsilon, Scale, Duration);
            ScaleIn = new EaseIn3(scaleIn);

            var calmExplosion = new GameTaskFunc(this, () => IsExploding = false);

            Fade = new FadeTo(this, 0, FadeTime);

            Sequence = new Sequence(ScaleIn, calmExplosion, Fade);
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            if (!Sequence.IsFinished)
            {
                Sequence.RunFrame(deltaTime);

                foreach(var enemy in ManagedEnemies)
                    enemy.RetributionBulletCollisionStay(this);
            }
            else
                DeactivateSelf();
        }

        #region Collision

        protected override void OnPlayerBulletTriggerEnter2D(Collider2D collision)
        {
            if (CollisionUtil.IsEnemyBullet(collision) && IsExploding)
            {
                var enemyBullet = collision.GetComponent<EnemyBullet>();
                enemyBullet.DeactivateSelf();
            }
        }

        public override bool CollidesWithEnemy(Enemy enemy)
        {
            bool alreadyManaged = ManagedEnemies.Contains(enemy);
            return !alreadyManaged;
        }

        public override void OnCollideWithEnemy(Enemy enemy)
        {
            ManagedEnemies.Add(enemy);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (CollisionUtil.IsEnemy(collision.otherCollider))
            {
                Enemy enemy = collision.otherCollider.GetComponent<Enemy>();
                enemy.RetributionBulletCollisionExit(this);
            }
        }

        #endregion Collision

        protected override void OnDeactivate()
        {
            transform.localScale = Vector3.zero;
        }
    }
}