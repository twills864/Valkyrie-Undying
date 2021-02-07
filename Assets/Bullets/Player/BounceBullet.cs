using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class BounceBullet : PlayerBullet
    {
        [SerializeField]
        private int DamageAfterBounce;
        [SerializeField]
        public float Speed;
        [SerializeField]
        private float RotationSpeed;

        public int BouncesLeft { get; set; }

        private int CurrentDamage { get; set; }
        public override int Damage => CurrentDamage;

        private Vector2 DefaultVelocity => new Vector2(0,Speed);


        public override void RunFrame(float deltaTime)
        {
            transform.Translate(deltaTime * Velocity, Space.World);
            transform.Rotate(0, 0, deltaTime * RotationSpeed);
        }

        protected override void OnActivate()
        {
            CurrentDamage = BaseDamage;
            Velocity = DefaultVelocity;
        }

        public override void OnCollideWithEnemy(Enemy enemy)
        {
            if(BouncesLeft > 0)
                Bounce(enemy);
            else
                DeactivateSelf();
        }

        private void OnBounce()
        {
            BouncesLeft--;
            CurrentDamage = DamageAfterBounce;
        }

        #region Enemy Bounce

        private void Bounce(Enemy enemy)
        {
            OnBounce();

            if (GameManager.Instance.TryGetRandomEnemyExcluding(enemy, out Enemy newTarget))
                SetTarget(newTarget);
            else
                Velocity = DefaultVelocity;
        }

        private void SetTarget(Enemy newTarget)
        {
            Velocity = MathUtil.VelocityVector(transform.position, newTarget.transform.position, Speed)
                        + newTarget.Velocity;
        }

        #endregion Enemy Bounce


        #region Max Level Screen Collision Bounce

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (CollisionUtil.IsScreenEdge(collision))
            {
                // Edge case preventing us from calling DeactivateSelf() here if BouncesLeft > 0 fails:
                // Imagine the bullet touches the screen edge, doesn't have any bounces left, and gets deactivated.
                // The bullet could theoretically miss an enemy behind the screen edge that it should have hit
                // under normal deactivation rules.
                if(IsMaxLevel && BouncesLeft > 0)
                    BounceOffScreenEdge();
            }
        }

        private void BounceOffScreenEdge()
        {
            OnBounce();

            if (GameManager.Instance.TryGetRandomEnemy(out Enemy newTarget))
                SetTarget(newTarget);
            else
                SetTargetToCenter();
        }

        private void SetTargetToCenter()
        {
            Velocity = MathUtil.VelocityVector(transform.position, SpaceUtil.WorldMap.Center, Speed);
        }

        #endregion Max Level Screen Collision Bounce
    }
}