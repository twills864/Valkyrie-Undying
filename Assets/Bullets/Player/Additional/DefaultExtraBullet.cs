using System;
using System.Linq;
using Assets.Util;
using Assets.Constants;
using Assets.GameTasks;
using Assets.ObjectPooling;
using UnityEngine;
using Assets.Enemies;
using Assets.Powerups.DefaultBulletBuff;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// The bullet spawned by default weapon powerups.
    /// </summary>
    /// <inheritdoc/>
    public class DefaultExtraBullet : DefaultInfluencedBullet
    {
        public override int Damage => BulletDamage;
        public override AudioClip FireSound => SoundBank.Silence;

        #region Prefabs

        #endregion Prefabs


        #region Prefab Properties

        #endregion Prefab Properties

        private int BulletDamage { get; set; }
        public PooledObjectTracker<Enemy> ParentEnemy { get; private set; }

        protected override void OnDefaultInfluencedBulletInit()
        {
            ParentEnemy = new PooledObjectTracker<Enemy>();
        }

        public override bool CollidesWithEnemy(Enemy enemy)
        {
            bool ret = !ParentEnemy.IsTarget(enemy);
            return ret;
        }

        protected override void OnDefaultInfluencedBulletCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            DefaultBulletBuffs.OnDefaultExtraBulletHit(this, enemy, hitPosition);
        }

        public static DefaultExtraBullet SpawnNew(Vector3 hitPosition, Vector2 velocity, DefaultBullet sourceBullet, Enemy hitEnemy)
        {
            float newScale = sourceBullet.DefaultExtraBulletScaleRatio;
            return SpawnNew(hitPosition, velocity, sourceBullet, hitEnemy, newScale);
        }

        public static DefaultExtraBullet SpawnNew(Vector3 hitPosition, Vector2 velocity, DefaultBullet sourceBullet, Enemy hitEnemy, float sizeScaleRatio)
        {
            DefaultExtraBullet bullet = PoolManager.Instance.BulletPool.Get<DefaultExtraBullet>(hitPosition, velocity);
            bullet.BulletDamage = sourceBullet.ExtraBulletDamage;

            bullet.ParentEnemy.Target = hitEnemy;
            bullet.OnSpawn();

            return bullet;
        }
    }
}