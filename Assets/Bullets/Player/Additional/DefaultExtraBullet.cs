using System;
using System.Linq;
using Assets.Util;
using Assets.Constants;
using Assets.GameTasks;
using Assets.ObjectPooling;
using UnityEngine;
using Assets.Enemies;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// The bullet spawned by default weapon powerups.
    /// </summary>
    /// <inheritdoc/>
    public class DefaultExtraBullet : DefaultInfluencedBullet
    {
        public override AudioClip FireSound => SoundBank.Silence;

        #region Prefabs

        #endregion Prefabs


        #region Prefab Properties

        #endregion Prefab Properties

        public PooledObjectTracker<Enemy> ParentEnemy { get; private set; }


        protected override void OnDefaultInfluencedBulletInit()
        {
            ParentEnemy = new PooledObjectTracker<Enemy>();
        }

        protected override void OnDefaultInfluencedBulletActivate()
        {

        }

        protected override void OnDefaultInfluencedBulletSpawn()
        {

        }

        protected override void OnDefaultInfluencedBulletFrameRun(float deltaTime, float realDeltaTime)
        {

        }

        public override bool CollidesWithEnemy(Enemy enemy)
        {
            bool ret = !ParentEnemy.IsTarget(enemy);
            return ret;
        }

        public static DefaultExtraBullet SpawnNew(Vector3 hitPosition, Vector2 velocity, DefaultBullet sourceBullet, Enemy hitEnemy)
        {
            float newScale = sourceBullet.DefaultExtraBulletScaleRatio;
            return SpawnNew(hitPosition, velocity, sourceBullet, hitEnemy, newScale);
        }

        public static DefaultExtraBullet SpawnNew(Vector3 hitPosition, Vector2 velocity, DefaultBullet sourceBullet, Enemy hitEnemy, float sizeScaleRatio)
        {
            DefaultExtraBullet bullet = PoolManager.Instance.BulletPool.Get<DefaultExtraBullet>(hitPosition, velocity);
            bullet.InitFromDefault(sourceBullet, sourceBullet.ExtraBulletDamage);

            bullet.ParentEnemy.Target = hitEnemy;

            bullet.LocalScale = sizeScaleRatio * sourceBullet.LocalScale;

            bullet.OnSpawn();

            return bullet;
        }
    }
}