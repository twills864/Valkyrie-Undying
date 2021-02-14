using Assets.Enemies;
using Assets.ScreenEdgeColliders;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class TrampolineBullet : BouncingBullet
    {
        [SerializeField]
        private float RotationSpeed;
        [SerializeField]
        private float BounceXVariance;

        protected override void OnPlayerBulletFrameRun(float deltaTime)
        {
            transform.Rotate(0, 0, deltaTime * RotationSpeed);
        }

        protected override void OnBounce(Enemy enemy)
        {
            RandomizeVelocityX();
        }

        private void RandomizeVelocityX()
        {
            var velocityX = RandomUtil.Float(-BounceXVariance, BounceXVariance);
            Velocity = new Vector2(velocityX, Velocity.y);
        }

        private void ReverseVelocityX()
        {
            Velocity = new Vector2(-Velocity.x, Velocity.y);
        }

        private void ResetVelocityY()
        {
            var velocityY = Speed;
            Velocity = new Vector2(Velocity.x, velocityY);
        }

        private void ReverseVelocityY()
        {
            Velocity = new Vector2(Velocity.x, -Velocity.y);
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (CollisionUtil.IsScreenEdge(collision, out ScreenSide screenSide))
            {
                switch(screenSide)
                {
                    case ScreenSide.Left:
                    case ScreenSide.Right:
                        ReverseVelocityX();
                        break;
                    case ScreenSide.Top:
                    case ScreenSide.Bottom:
                        ReverseVelocityY();
                        break;
                    default:
                        throw ExceptionUtil.ArgumentException(screenSide);
                }
                BounceOffScreenEdge();
            }
            else if(CollisionUtil.IsPlayer(collision))
            {
                ResetVelocityY();
            }
        }

        private void BounceOffScreenEdge()
        {
            //ApplyBounceDebuff();

            ReverseVelocityX();

            //if (GameManager.Instance.TryGetRandomEnemy(out Enemy newTarget))
            //    SetTarget(newTarget);
            //else
            //    SetTargetToCenter();
        }
    }
}