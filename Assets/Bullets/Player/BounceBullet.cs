using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class BounceBullet : PlayerBullet
    {
        [SerializeField]
        public float Speed;
        [SerializeField]
        private float RotationSpeed;

        public int BouncesLeft { get; set; }


        public override void RunFrame(float deltaTime)
        {
            transform.Translate(deltaTime * Velocity);
            //GetComponent<Rigidbody2D>().rotation += deltaTime * RotationSpeed;

            //transform.Rotate(0, 0, deltaTime * RotationSpeed);

            // This line is incorrect, but produces entertaining results that you should explore later.
            //transform.Rotate(deltaTime * RotationSpeed, 0, 0);
        }

        protected override void OnActivate()
        {
            transform.rotation = Quaternion.identity;
            base.OnActivate();
        }

        public override void OnCollideWithEnemy(Enemy enemy)
        {
            if(BouncesLeft > 0)
            {
                BouncesLeft--;

                if (GameManager.Instance.TryGetRandomEnemyExcluding(enemy, out Enemy newTarget))
                    SetTarget(newTarget);
                else
                    Velocity = new Vector2(0, Speed);
            }
            else
                DeactivateSelf();
        }

        private void SetTarget(Enemy newTarget)
        {
            Velocity = MathUtil.VelocityVector(transform.position, newTarget.transform.position, Speed);
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (CollisionUtil.IsPlayerBullet(collision))
            {
                var bullet = collision.GetComponent<PlayerBullet>();
                if (bullet.GetType() != typeof(BounceBullet))
                    MarkSelfCollision();
            }
        }
    }
}