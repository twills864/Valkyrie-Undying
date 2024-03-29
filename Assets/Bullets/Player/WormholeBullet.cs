﻿using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// A bullet that will warp through an enemy, and will warp from the
    /// top of the screen back to the bottom of the screen.
    /// The number of times this bullet can warp will increase with the bullet level.
    /// </summary>
    /// <inheritdoc/>
    public class WormholeBullet : PlayerBullet
    {
        protected override bool AutomaticallyDeactivate => false;
        protected override bool ShouldMarkSelfCollision => false;
        public override AudioClip FireSound => SoundBank.LaserPuff;

        #region Prefabs

        [SerializeField]
        private float _StartSpeed = GameConstants.PrefabNumber;

        [SerializeField]
        private float _EndSpeed = GameConstants.PrefabNumber;

        [SerializeField]
        private float _AccelerationTime = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private float StartSpeed => _StartSpeed;
        private float EndSpeed => _EndSpeed;

        private float AccelerationTime => _AccelerationTime;

        #endregion Prefab Properties


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

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
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
                DetachBulletTrail();

                var pos = new Vector3(transform.position.x, SpaceUtil.WorldMap.Bottom.y, 0);
                transform.position = pos;

                AttachNewBulletTrail();
            }
        }

        protected override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
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