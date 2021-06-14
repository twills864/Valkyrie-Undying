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

        public int PoisonDamage => SnakeBiteDamage + VenomousRoundsDamage;

        [Obsolete(ObsoleteConstants.FollowTheFun)]
        public int SnakeBiteDamage { get; set; }
        public int VenomousRoundsDamage { get; set; }

        public int ParasiteDamage { get; set; }

        public static void StaticInit()
        {
            MaxPenetration = 0;
        }

        #region Prefabs

        [SerializeField]
        private float _DamageScale = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public float DamageScale => _DamageScale;

        #endregion Prefab Properties

        #region Penetration

        public static int MaxPenetration { get; set; }
        private int NumberPenetrated { get; set; }

        protected override bool AutomaticallyDeactivate => NumberPenetrated >= MaxPenetration;

        #endregion Penetration

        public PooledObjectTracker<Enemy> ParentEnemy { get; private set; }


        protected override void OnPlayerBulletInit()
        {
            ParentEnemy = new PooledObjectTracker<Enemy>();
        }

        protected override void OnActivate()
        {
            NumberPenetrated = 0;
            VenomousRoundsDamage = 0;
            SnakeBiteDamage = 0;
            ParasiteDamage = 0;
        }

        protected override void OnBulletSpawn()
        {

        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {

        }

        public override bool CollidesWithEnemy(Enemy enemy)
        {
            bool ret = !ParentEnemy.IsTarget(enemy);
            return ret;
        }

        protected override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            if (SnakeBiteDamage > 0)
                enemy.AddPoison(SnakeBiteDamage);

            if (ParasiteDamage > 0)
                enemy.AddParasites(ParasiteDamage);

            NumberPenetrated++;
        }

        public static DefaultExtraBullet SpawnNew(Vector3 hitPosition, Vector2 velocity, DefaultBullet sourceBullet, Enemy hitEnemy)
        {
            float newScale = sourceBullet.DefaultExtraBulletScaleRatio;
            return SpawnNew(hitPosition, velocity, sourceBullet, hitEnemy, newScale);
        }

        public static DefaultExtraBullet SpawnNew(Vector3 hitPosition, Vector2 velocity, DefaultBullet sourceBullet, Enemy hitEnemy, float sizeScaleRatio)
        {
            DefaultExtraBullet bullet = PoolManager.Instance.BulletPool.Get<DefaultExtraBullet>(hitPosition, velocity);

            bullet.ParentEnemy.Target = hitEnemy;

            bullet.DefaultExtraDamage = (int) (sourceBullet.Damage * bullet.DamageScale);

            bullet.SnakeBiteDamage = sourceBullet.SnakeBiteDamage / 2;
            bullet.ParasiteDamage = sourceBullet.ParasiteDamage / 2;

            bullet.LocalScale = sizeScaleRatio * sourceBullet.LocalScale;

            bullet.OnSpawn();

            return bullet;
        }
    }
}