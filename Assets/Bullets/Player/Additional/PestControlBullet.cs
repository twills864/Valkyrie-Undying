﻿using Assets.Bullets.EnemyBullets;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class PestControlBullet : PlayerBullet
    {
        [SerializeField]
        private float Speed = GameConstants.PrefabNumber;

        protected override bool ShouldMarkSelfCollision => false;

        protected override void OnPlayerBulletTriggerEnter2D(Collider2D collision)
        {
            if(CollisionUtil.IsEnemyBullet(collision))
            {
                var enemyBullet = collision.GetComponent<EnemyBullet>();
                GameManager.Instance.ReflectBulletFromPestControl(enemyBullet, this);
                DeactivateSelf();
            }
        }

        public void SetTarget(Vector2 spawnPosition, EnemyBullet enemyBullet)
        {
            transform.position = spawnPosition;
            var targetPos = enemyBullet.transform.position;

            var velocity = MathUtil.VelocityVector(spawnPosition, targetPos, Speed);

            // Crude homing - add enemy bullet velocity
            velocity += enemyBullet.Velocity;

            Velocity = velocity;
        }
    }
}