using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.GameTasks;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.EnemyBullets
{
    /// <summary>
    /// Represents any bullet that can be fired by an enemy
    /// and will damage the player if it collides with them.
    /// </summary>
    /// <inheritdoc/>
    public abstract class EnemyBullet : Bullet
    {
        public override string LogTagColor => "#FFA197";
        public override TimeScaleType TimeScale => TimeScaleType.EnemyBullet;

        #region Prefabs

        [SerializeField]
        private int _ReflectDamage = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public int ReflectDamage => _ReflectDamage;

        #endregion Prefab Properties


        public virtual bool CanReflect => true;
        public virtual bool HitsPlayer => true;
        public virtual bool DeactivateOnHit => true;

        public override AudioClip FireSound => SoundBank.GunPistol;

        protected virtual void OnEnemyBulletInit() { }
        protected sealed override void OnBulletInit()
        {
            OnEnemyBulletInit();
        }

        protected virtual void OnEnemyBulletActivate() { }
        protected sealed override void OnActivate()
        {
            OnEnemyBulletActivate();
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if(CollisionUtil.IsPlayer(collision))
            {
                if (HitsPlayer && Player.Instance.CollidesWithBullet(this) && DeactivateOnHit)
                    DeactivateSelf();
            }
        }
    }
}
