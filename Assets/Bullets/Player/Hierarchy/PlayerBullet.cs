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
        public override TimeScaleType TimeScale => TimeScaleType.PlayerBullet;

        #region Prefabs

        [SerializeField]
        private int _BaseDamage = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        protected int BaseDamage => _BaseDamage;

        #endregion Prefab Properties


        public virtual int Damage => BaseDamage;

        public int BulletLevel { get; set; }
        public bool IsMaxLevel => BulletLevel == GameConstants.MaxWeaponLevel;
        protected int BulletLevelPlusOneIfMax
            => !IsMaxLevel ? BulletLevel : BulletLevel + 1;

        protected virtual bool ShouldMarkSelfCollision => true;

        public virtual AudioClip HitSound => SoundBank.ExplosionShortDeep;
        public virtual float HitSoundVolume => 0.2f;
        public void PlayHitSound() => PlaySoundAtCenter(HitSound, HitSoundVolume);

        public virtual bool BouncesTrampolineBullet => false;

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

        public virtual void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            PlayHitSound();
            DeactivateSelf();
        }

        protected virtual void OnPlayerBulletTriggerExit2D(Collider2D collision) { }
        protected sealed override void OnBulletTriggerExit2D(Collider2D collision)
        {
            OnPlayerBulletTriggerExit2D(collision);
        }


        protected virtual void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime) { }
        protected sealed override void OnFrameRun(float deltaTime, float realDeltaTime)
        {
            OnPlayerBulletFrameRun(deltaTime, realDeltaTime);
        }

        public virtual bool CollidesWithEnemy(Enemy enemy)
        {
            return true;
        }

        public virtual Vector3 GetHitPosition(Enemy enemy)
        {
            Vector3 ret = transform.position;
            return ret;
        }

        /// <summary>
        /// Gets the closest point on the enemy's Collider2D to this bullet's transform.
        /// Intended to be used as an override to GetHitPosition().
        /// </summary>
        /// <param name="enemy">The enemy to find the closest point to.</param>
        /// <returns>The closest point on the enemy to this transform.</returns>
        protected Vector3 GetClosestPoint(Enemy enemy)
        {
            Vector3 ret = enemy.ColliderMap.Collider.ClosestPoint(transform.position);
            return ret;
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
            Log($"Bullet self-collision! {transform.position} {GetComponent<SpriteRenderer>().bounds.center} {thisBulletType.Name} -> {collidingBulletType.Name}");
            DebugUtil.RedX(transform.position, 0.5f);
        }

        #region Common Overrides

        public bool ActivateOnCollideWithoutColliding(Enemy enemy)
        {
            Vector3 hitPosition = GetHitPosition(enemy);
            OnCollideWithEnemy(enemy, hitPosition);
            return false;
        }

        #endregion Common Overrides
    }
}
