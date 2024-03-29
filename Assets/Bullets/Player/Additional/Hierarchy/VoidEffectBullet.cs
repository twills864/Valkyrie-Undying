﻿using Assets.Bullets.EnemyBullets;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// Represents a bullet that will activate the Void Pause status effect
    /// while it's colliding with an enemy. This bullet will also either
    /// destroy or reflect enemy bullets that it collides with.
    /// </summary>
    /// <inheritdoc/>
    public abstract class VoidEffectBullet : PlayerBullet
    {
        protected override bool AutomaticallyDeactivate => false;
        protected sealed override bool ShouldDeactivateOnDestructor => false;
        protected sealed override bool ShouldMarkSelfCollision => false;

        protected virtual bool ShouldReflectBullet => true;

        protected float InitialScale => float.Epsilon;

        protected sealed override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
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