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
        private float _ScaleInTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _FadeTime = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private float FadeTime => _FadeTime;
        private float ScaleInTime => _ScaleInTime;

        #endregion Prefab Properties

        public float RetributionTimescale => 1.0f - Alpha;

        private float Scale { get; set; }

        private bool IsExploding { get; set; }

        private MoveTo MoveToCenter { get; set; }
        private Sequence Sequence { get; set; }

        private TrackedPooledObjectSet<Enemy> ManagedEnemies { get; set; }
        private TrackedPooledObjectSet<EnemyBullet> ManagedBullets { get; set; }

        public static RetributionBullet StartRetribution(Vector3 position)
        {
            var bullet = PoolManager.Instance.BulletPool.Get<RetributionBullet>(position);
            bullet.OnSpawn();

            return bullet;
        }

        protected override void OnPermanentVelocityBulletInit()
        {
            Scale = SpaceUtil.WorldMap.Height;

            var scaleIn = new ScaleTo(this, float.Epsilon, Scale, ScaleInTime);
            var scaleEase = new EaseIn3(scaleIn);

            MoveToCenter = new MoveTo(this, Vector3.zero, new Vector3(1f, 1f), ScaleInTime);

            var scaleAndMove = new ConcurrentGameTask(this, scaleEase, MoveToCenter);

            var calmExplosion = new GameTaskFunc(this, () => IsExploding = false);

            var fade = new FadeTo(this, 0, FadeTime);
            var fadeEase = new EaseOut3(fade);

            Sequence = new Sequence(scaleAndMove, calmExplosion, fadeEase);

            ManagedEnemies = new TrackedPooledObjectSet<Enemy>();
            ManagedBullets = new TrackedPooledObjectSet<EnemyBullet>();
        }

        protected override void OnActivate()
        {
            Color color = Sprite.color;
            color.a = 1.0f;
            Sprite.color = color;

            IsExploding = true;
            ManagedEnemies.Clear();

            Sequence.ResetSelf();
        }

        public override void OnSpawn()
        {
            MoveToCenter.ReinitializeMove(transform.position, SpaceUtil.WorldMap.Center);
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            if (!Sequence.IsFinished)
            {
                Sequence.RunFrame(deltaTime);

                foreach(var enemy in ManagedEnemies)
                    enemy.RetributionBulletCollisionStay(this);

                foreach(var bullet in ManagedBullets)
                    bullet.RetributionBulletCollisionStay(this);
            }
            else
                DeactivateSelf();
        }

        #region Collision

        protected override void OnPlayerBulletTriggerEnter2D(Collider2D collision)
        {
            if (CollisionUtil.IsEnemyBullet(collision))
            {
                var enemyBullet = collision.GetComponent<EnemyBullet>();
                if (IsExploding)
                    enemyBullet.DeactivateSelf();
                else
                    ManagedBullets.Add(enemyBullet);
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