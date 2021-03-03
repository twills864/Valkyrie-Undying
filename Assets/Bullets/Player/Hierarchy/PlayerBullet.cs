using System;
using Assets.Constants;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public abstract class PlayerBullet : Bullet
    {
        public override string LogTagColor => "#B381FE";
        public override GameTaskType TaskType => GameTaskType.Bullet;

        [SerializeField]
        protected int BaseDamage;
        public virtual int Damage => BaseDamage;
        public virtual float PestControlChance => Damage * 0.01f;

        public int BulletLevel { get; set; }
        public bool IsMaxLevel => BulletLevel == GameConstants.MaxWeaponLevel;
        protected int BulletLevelPlusOneIfMax
            => !IsMaxLevel ? BulletLevel : BulletLevel + 1;

        protected virtual bool ShouldMarkSelfCollision => true;

        protected virtual void OnPlayerBulletInit() { }
        protected sealed override void OnBulletInit()
        {
            OnPlayerBulletInit();
        }

        protected virtual void OnPlayerBulletTriggerEnter2D(Collider2D collision) { }
        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (CollisionUtil.IsPlayerBullet(collision) && ShouldMarkSelfCollision)
            {
                var otherBullet = collision.GetComponent<PlayerBullet>();

                if (otherBullet.ShouldMarkSelfCollision)
                {
                    var thisType = this.GetType();
                    var otherType = otherBullet.GetType();

                    if(thisType == otherType)
                        MarkSelfCollision(thisType, otherType);
                }
            }

            OnPlayerBulletTriggerEnter2D(collision);
        }

        public virtual void OnCollideWithEnemy(Enemy enemy)
        {
            DeactivateSelf();
        }

        protected virtual void OnPlayerBulletTriggerExit2D(Collider2D collision) { }
        protected sealed override void OnBulletTriggerExit2D(Collider2D collision)
        {
            OnPlayerBulletTriggerExit2D(collision);
        }

        protected virtual void OnPlayerBulletFrameRun(float deltaTime) { }
        protected sealed override void OnManagedVelocityObjectFrameRun(float deltaTime)
        {
            OnPlayerBulletFrameRun(deltaTime);
        }

        public virtual bool CollidesWithEnemy(Enemy enemy)
        {
            return true;
        }

        /// <summary>
        /// Debugging method that visualizes the location of this PlayerBullet
        /// colliding with another PlayerBullet.
        /// In some cases, like the shotgun, care should be taken to avoid
        /// bullets colliding with each other on spawn in order to
        /// save calls to the collision detection methods.
        /// </summary>
        /// <param name="thisBulletType">The type of bullet initiating the self-collision mark.</param>
        /// <param name="collidingBulletType">The type of bullet that collided with the current bullet.</param>
        protected void MarkSelfCollision(Type thisBulletType, Type collidingBulletType)
        {
            Log($"Bullet self-collision! {thisBulletType.Name} -> {collidingBulletType.Name}");
            DebugUtil.RedX(transform.position, 0.5f);
        }
    }
}
