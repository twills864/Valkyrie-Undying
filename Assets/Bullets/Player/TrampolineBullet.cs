using Assets.Constants;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.ScreenEdgeColliders;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// A bullet that travels across the screen with a y-position described by the following function:
    /// f(t) = -(t-sqrt(mapHeight))^2 + mapHeight
    ///
    /// This bullet will obtain a random x-velocity after hitting an enemy,
    /// and can bounce upward again by colliding with the player, or any relevant powerups.
    /// </summary>
    /// <inheritdoc/>
    public class TrampolineBullet : BouncingBullet
    {
        private const float ScreenBuffer = Destructor.Buffer;

        public override float BulletTrailWidth => SpriteMap.Width * 0.8f;
        public override AudioClip FireSound => SoundBank.ExplosionShortestKnock;

        // f(t) = -(t-sqrt(mapHeight))^2 + mapHeight
        // f'(t) = -2(t - sqrt(mapHeight))
        public override Vector2 RepresentedVelocity
        {
            get
            {
                float x = VelocityX;
                float y = -2f * (ElapsedTime - Mathf.Sqrt(SpaceUtil.WorldMapSize.y));

                Vector2 velocity = new Vector2(x, y);
                return velocity;
            }
        }

        #region Prefabs

        [SerializeField]
        private float _RotationSpeed = GameConstants.PrefabNumber;
        [SerializeField]
        private float _BounceXVarianceMin = GameConstants.PrefabNumber;
        [SerializeField]
        private float _BounceXVarianceMax = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private float RotationSpeed => _RotationSpeed;
        private float BounceXVarianceMin => _BounceXVarianceMin;
        private float BounceXVarianceMax => _BounceXVarianceMax;

        #endregion Prefab Properties


        private float ElapsedTime { get; set; }
        private float SqrtMapHeight { get; set; }
        private float SpriteHeight { get; set; }

        // Scales the velocity of the bullet so that it will have a
        // velocity equal to Speed at f(0)
        // f'(t) = -2(t - sqrt(mapHeight))
        // f'(0) = Speed = 2(sqrt(mapHeight)) * SpeedScaled
        // SpeedScaled = Speed / 2(sqrt(mapHeight))
        private float SpeedScaled { get; set; }

        protected override void OnBouncingBulletInit()
        {
            SqrtMapHeight = Mathf.Sqrt(SpaceUtil.WorldMapSize.y);
            SpeedScaled = Speed / (2 * SqrtMapHeight);
            SpriteHeight = GetComponent<SpriteRenderer>().size.y / 4;
        }

        protected override void OnBouncingBulletSpawn()
        {
            transform.position += new Vector3(0, SpriteHeight, 0);
            ResetVelocityY();
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            ElapsedTime += (SpeedScaled * deltaTime);
            transform.Rotate(0, 0, deltaTime * RotationSpeed);

            // Recalculate y position
            var x2 = (ElapsedTime - SqrtMapHeight);
            x2 *= -x2;
            float newY = x2 + SpaceUtil.WorldMap.Top.y;
            transform.position = new Vector3(transform.position.x, newY, 0);
        }

        protected override void OnBounce(Enemy enemy)
        {
            RandomizeVelocityX();
        }

        private void RandomizeVelocityX()
        {
            var velocityX = RandomUtil.Float(BounceXVarianceMin, BounceXVarianceMax);
            velocityX = RandomUtil.RandomlyNegative(velocityX);
            Velocity = new Vector2(velocityX, Velocity.y);
        }

        private void ReverseVelocityX()
        {
            Velocity = new Vector2(-Velocity.x, Velocity.y);
        }

        private void ResetVelocityY()
        {
            //dt = sqrt(MAP - y) + sqrt(MAP)
            ElapsedTime = - Mathf.Sqrt(SpaceUtil.WorldMap.Top.y - transform.position.y) + SqrtMapHeight;
        }

        private void BounceOffPlayer()
        {
            ResetVelocityY();
            RandomizeVelocityX();
        }

        protected override void OnPlayerBulletTriggerEnter2D(Collider2D collision)
        {
            if (CollisionUtil.IsScreenEdge(collision, out ScreenSide screenSide))
            {
                BounceOffScreenEdge(screenSide);
            }
            else if(CollisionUtil.IsPlayer(collision))
            {
                BounceOffPlayer();
            }
            else if (CollisionUtil.IsPlayerBullet(collision))
            {
                const float RoughTravellingDownwardsCheck = 1.0f;
                if (ElapsedTime > RoughTravellingDownwardsCheck)
                {
                    var playerBullet = collision.GetComponent<PlayerBullet>();
                    if (playerBullet.BouncesTrampolineBullet)
                        BounceOffPlayer();
                }
            }
        }

        private void BounceOffScreenEdge(ScreenSide screenSide)
        {
            switch (screenSide)
            {
                case ScreenSide.Left:
                case ScreenSide.Right:
                    ReverseVelocityX();
                    break;
                case ScreenSide.Top:
                case ScreenSide.Bottom:
                    //ReverseVelocityY();
                    break;
                default:
                    throw ExceptionUtil.ArgumentException(() => screenSide);
            }
        }
    }
}