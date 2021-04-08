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

        #region Prefabs

        [SerializeField]
        private int _ReflectDamage;

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

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if(CollisionUtil.IsPlayer(collision))
            {
                if (HitsPlayer && Player.Instance.CollideWithBullet(this) && DeactivateOnHit)
                    DeactivateSelf();
            }
        }
    }
}
