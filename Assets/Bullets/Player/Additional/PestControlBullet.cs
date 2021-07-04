using System;
using Assets.Bullets.EnemyBullets;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// The bullet fired by the now-deprecated Pest Control powerup.
    /// </summary>
    /// <inheritdoc/>
    [Obsolete(ObsoleteConstants.FollowTheFun)]
    public class PestControlBullet : PlayerBullet
    {
        protected override bool ShouldMarkSelfCollision => false;
        public override AudioClip FireSound => SoundBank.GunShort;

        #region Prefabs

        [SerializeField]
        private float _Speed = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public float Speed => _Speed;

        #endregion Prefab Properties


        protected override void OnPlayerBulletTriggerEnter2D(Collider2D collision)
        {
            if(CollisionUtil.IsEnemyBullet(collision))
            {
                var enemyBullet = collision.GetComponent<EnemyBullet>();
                GameManager.Instance.ReflectBulletFromPestControl(enemyBullet, this);
                DeactivateSelf();
            }
        }

        public void SetTarget(Vector3 spawnPosition, EnemyBullet enemyBullet)
        {
            transform.position = spawnPosition;
            var targetPos = enemyBullet.transform.position;

            var velocity = MathUtil.VelocityVector(spawnPosition, targetPos, Speed);

            // Crude homing - add enemy bullet velocity
            velocity += enemyBullet.Velocity;

            Velocity = velocity;

            OnSpawn();
        }
    }
}