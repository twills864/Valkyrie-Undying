using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public abstract class PlayerBullet : Bullet
    {
        public override string LogTagColor => "#B381FE";

        [SerializeField]
        protected int BaseDamage;
        public virtual int Damage => BaseDamage;

        protected virtual void OnPlayerBulletInit() { }
        protected sealed override void OnBulletInit()
        {
            OnPlayerBulletInit();
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (CollisionUtil.IsPlayerBullet(collision))
                MarkSelfCollision();
        }

        public virtual void OnCollideWithEnemy(Enemy enemy)
        {
            DeactivateSelf();
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
            DebugUtil.RedX(transform.position, "Player bullet collide");
        }
    }
}
