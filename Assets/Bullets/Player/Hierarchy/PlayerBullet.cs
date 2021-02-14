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

        public int BulletLevel { get; set; }
        public bool IsMaxLevel => BulletLevel == GameConstants.MaxWeaponLevel;

        protected virtual bool ShouldMarkSelfCollision => true;

        protected virtual void OnPlayerBulletInit() { }
        protected sealed override void OnBulletInit()
        {
            OnPlayerBulletInit();
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (CollisionUtil.IsPlayerBullet(collision) && ShouldMarkSelfCollision)
                MarkSelfCollision();
        }

        public virtual void OnCollideWithEnemy(Enemy enemy)
        {
            DeactivateSelf();
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
        protected void MarkSelfCollision()
        {
            Log("Bullet self-collision!");
            DebugUtil.RedX(transform.position, 0.5f);
        }
    }
}
