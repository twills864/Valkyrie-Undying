using System;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Powerups;
using Assets.ScreenEdgeColliders;
using Assets.UnityPrefabStructs;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// Encapsulates all bullets influenced by the current state of default bullets.
    /// </summary>
    /// <inheritdoc/>
    public abstract class DefaultInfluencedBullet : PlayerBullet
    {
        #region Property Fields
        private FireDamage _FireDamage = default;
        #endregion Property Field

        public sealed override int Damage => BulletDamage;
        public int BulletDamage { get; set; }

        #region Prefabs

        #endregion Prefabs


        #region Prefab Properties

        #endregion Prefab Properties


        #region Penetration

        public int BulletPenetration { get; protected set; }
        private int NumberPenetrated { get; set; }
        protected sealed override bool AutomaticallyDeactivate => NumberPenetrated >= BulletPenetration;

        #endregion Penetration

        public int PoisonDamage { get; protected set; }
        public int ParasiteDamage { get; protected set; }

        #region FireDamage

        public bool IsBurning => FireCollisionDamage > 0;

        public int FireCollisionDamage
        {
            get => _FireDamage.CollisionDamage;
            protected set => _FireDamage.CollisionDamage = value;
        }

        public int FireDamageIncreasePerTick
        {
            get => _FireDamage.DamageIncreasePerTick;
            protected set => _FireDamage.DamageIncreasePerTick = value;
        }

        public int FireMaxDamage
        {
            get => _FireDamage.MaxDamage;
            protected set => _FireDamage.MaxDamage = value;
        }

        #endregion FireDamage

        public int AcidDamage { get; protected set; }
        public int ChilledTime { get; protected set; }
        public int SilenceTime { get; protected set; }

        protected virtual void OnDefaultInfluencedBulletInit() { }
        protected sealed override void OnPlayerBulletInit()
        {
            OnDefaultInfluencedBulletInit();
        }

        protected virtual void OnDefaultInfluencedBulletActivate() { }
        protected sealed override void OnActivate()
        {
            BulletDamage = BaseDamage;

            NumberPenetrated = 0;
            BulletPenetration = 0;

            PoisonDamage = 0;
            ParasiteDamage = 0;
            _FireDamage.Reset();
            AcidDamage = 0;
            ChilledTime = 0;
            SilenceTime = 0;

            OnDefaultInfluencedBulletActivate();
        }

        protected virtual void OnDefaultInfluencedBulletSpawn() { }
        protected sealed override void OnBulletSpawn()
        {
            OnDefaultInfluencedBulletSpawn();
        }

        protected virtual void OnDefaultInfluencedBulletFrameRun(float deltaTime, float realDeltaTime) { }
        protected sealed override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            OnDefaultInfluencedBulletFrameRun(deltaTime, realDeltaTime);
        }

        protected virtual void OnDefaultInfluencedBulletCollideWithEnemy(Enemy enemy, Vector3 hitPosition) { }
        protected sealed override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            OnDefaultInfluencedBulletCollideWithEnemy(enemy, hitPosition);

            if (PoisonDamage > 0)
                enemy.AddPoison(PoisonDamage);

            if (ParasiteDamage > 0)
                enemy.AddParasites(ParasiteDamage);

            if (IsBurning)
                enemy.Ignite(FireCollisionDamage, FireDamageIncreasePerTick, FireMaxDamage);

            //if (AcidDamage > 0)
            //    enemy.AddAcid(AcidDamage);

            //if (ChilledTime > 0)
            //    enemy.AddChill(ChilledTime);

            //if (SilenceTime > 0)
            //    enemy.Silence(SilenceTime);

            NumberPenetrated++;
        }

        protected virtual void OnDefaultInfluencedBulletTriggerEnter2D(Collider2D collision) { }
        protected sealed override void OnPlayerBulletTriggerEnter2D(Collider2D collision)
        {
            OnDefaultInfluencedBulletTriggerEnter2D(collision);
        }

        public void InitFromDefault(DefaultBullet def, int damage)
        {
            BulletDamage = damage;
            Velocity *= def.CalculateVelocityScale();

            NumberPenetrated = def.NumberPenetrated;
            BulletPenetration = DefaultBullet.MaxPenetration;

            PoisonDamage = def.PoisonDamage / 2;
            ParasiteDamage = def.ParasiteDamage / 2;
            //_FireDamage.Reset();
            //AcidDamage = def.AcidDamage / 2;
            //ChilledTime = def.ChilledTime / 2;
            //SilenceTime = def.SilenceTime / 2;
        }
    }
}