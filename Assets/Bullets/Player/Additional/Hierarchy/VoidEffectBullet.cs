using Assets.Bullets.EnemyBullets;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public abstract class VoidEffectBullet : PlayerBullet
    {
        protected sealed override bool ShouldDeactivateOnDestructor => false;
        protected sealed override bool ShouldMarkSelfCollision => false;

        protected virtual bool ShouldReflectBullet => true;

        protected float InitialScale => float.Epsilon;

        public sealed override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            enemy.VoidPause();
        }

        protected sealed override void OnPlayerBulletTriggerEnter2D(Collider2D collision)
        {
            if (CollisionUtil.IsEnemyBullet(collision))
            {
                var enemyBullet = collision.GetComponent<EnemyBullet>();

                if (ShouldReflectBullet)
                    GameManager.Instance.ReflectBullet(enemyBullet);
                else
                    enemyBullet.DeactivateSelf();
            }
        }

        protected sealed override void OnTriggerExit2D(Collider2D collision)
        {
            if(CollisionUtil.IsEnemy(collision))
            {
                var enemy = collision.GetComponent<Enemy>();
                enemy.VoidResume();
            }
        }
    }
}