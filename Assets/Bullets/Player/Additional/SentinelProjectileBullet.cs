using System;
using System.Linq;
using Assets.Util;
using Assets.Constants;
using Assets.GameTasks;
using Assets.ObjectPooling;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    ///
    /// </summary>
    /// <inheritdoc/>
    public class SentinelProjectileBullet : PlayerBullet
    {
        public override int Damage => SentinelProjectileDamage;

        #region Prefabs

        [SerializeField]
        private float _Speed = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public float Speed => _Speed;

        #endregion Prefab Properties


        public int SentinelProjectileDamage { get; set; }

        public void CloneFromSentinel(SentinelBullet sentinel)
        {
            SpriteColor = sentinel.SpriteColor;
            transform.localScale = sentinel.transform.localScale;
        }

        public static SentinelProjectileBullet GetProjectile(SentinelBullet sentinel)
        {
            SentinelProjectileBullet projectile = (SentinelProjectileBullet)PoolManager.Instance
                .BulletPool.Get<SentinelProjectileBullet>();

            projectile.transform.position = sentinel.transform.position;
            projectile.SentinelProjectileDamage = sentinel.Damage;

            return projectile;
        }
    }
}