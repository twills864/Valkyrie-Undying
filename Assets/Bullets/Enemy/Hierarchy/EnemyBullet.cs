using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.GameTasks;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.EnemyBullets
{
    /// <inheritdoc/>
    public abstract class EnemyBullet : Bullet
    {
        public override string LogTagColor => "#FFA197";
        public override TimeScaleType TimeScale => TimeScaleType.EnemyBullet;
        public override float TimeScaleModifier => base.TimeScaleModifier * RetributionTimeScale;

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

        protected virtual void OnEnemyBulletInit() { }
        protected sealed override void OnBulletInit()
        {
            OnEnemyBulletInit();
        }

        protected virtual void OnEnemyBulletActivate() { }
        protected sealed override void OnActivate()
        {
            RetributionTimeScale = 1.0f;
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

        protected float RetributionTimeScale { get; private set; }

        public void RetributionBulletCollisionStay(RetributionBullet bullet)
        {
            RetributionTimeScale = bullet.RetributionTimescale;
        }

        public void RetributionBulletCollisionExit(RetributionBullet bullet)
        {
            RetributionTimeScale = 1.0f;
        }
    }
}
