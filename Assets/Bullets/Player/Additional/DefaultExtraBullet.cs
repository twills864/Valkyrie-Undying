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
    public class DefaultExtraBullet : PlayerBullet
    {
        public override int Damage => DefaultExtraDamage;
        public override AudioClip FireSound => SoundBank.Silence;

        public int DefaultExtraDamage { get; set; }

        public PooledObjectTracker<Enemy> Parent { get; private set; }

        public static void StaticInit()
        {

        }

        protected override void OnPlayerBulletInit()
        {
            Parent = new PooledObjectTracker<Enemy>();
        }

        protected override void OnActivate()
        {

        }

        public override void OnSpawn()
        {

        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {

        }

        public override bool CollidesWithEnemy(Enemy enemy)
        {
            bool ret = !Parent.IsTarget(enemy);
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

            bullet.Parent.Target = hitEnemy;
            bullet.DefaultExtraDamage = sourceBullet.Damage;

            bullet.LocalScale = sizeScaleRatio * sourceBullet.LocalScale;

            return bullet;
        }
    }
}