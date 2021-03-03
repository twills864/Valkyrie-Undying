using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class WormholeBullet : PlayerBullet
    {
        [SerializeField]
        private float StartSpeed;

        [SerializeField]
        private float EndSpeed;

        [SerializeField]
        private float AccelerationTime;

        protected override bool ShouldMarkSelfCollision => false;

        private Vector2 StartVelocity => new Vector2(0, StartSpeed);
        private Vector2 EndVelocity => new Vector2(0, EndSpeed);
        private VelocityChange Accelerate { get; set; }

        private int WarpsLeft { get; set; }

        private Vector2? EnemyWarpDestination { get; set; }

        protected override void OnPlayerBulletInit()
        {
            Accelerate = new VelocityChange(this, StartVelocity, EndVelocity, AccelerationTime);
        }

        protected override void OnActivate()
        {
            Velocity = StartVelocity;
            WarpsLeft = BulletLevelPlusOneIfMax + 2;

            Accelerate.ResetSelf();
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime)
        {
            Accelerate.RunFrame(deltaTime);

            if (EnemyWarpDestination.HasValue)
            {
                if (EnemyWarpDestination.Value.y > transform.position.y)
                    transform.position = EnemyWarpDestination.Value;

                EnemyWarpDestination = null;
            }

            if (transform.position.y > SpaceUtil.WorldMap.Top.y && !WarpDebuffDeactivates())
            {
                var pos = new Vector3(transform.position.x, SpaceUtil.WorldMap.Bottom.y, 0);
                transform.position = pos;
            }
        }

        public override void OnCollideWithEnemy(Enemy enemy)
        {
            if(!WarpDebuffDeactivates())
                EnemyWarpDestination = new Vector3(transform.position.x, enemy.SpriteMap.Top.y, 0);
        }

        private bool WarpDebuffDeactivates()
        {
            bool ret;

            if(WarpsLeft == 0)
            {
                DeactivateSelf();
                ret = true;
            }
            else
            {
                WarpsLeft--;
                ret = false;
            }

            return ret;
        }
    }
}