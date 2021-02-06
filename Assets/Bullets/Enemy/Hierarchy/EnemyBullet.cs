using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.EnemyBullets
{
    /// <inheritdoc/>
    public abstract class EnemyBullet : Bullet
    {
        public override string LogTagColor => "#FFA197";

        protected virtual void OnEnemyBulletInit() { }
        protected sealed override void OnBulletInit()
        {
            OnEnemyBulletInit();
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if(CollisionUtil.IsPlayer(collision))
            {
                if (Player.Instance.CollideWithBullet(this))
                    DeactivateSelf();
            }
        }
    }
}
