using System;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Powerups;
using Assets.Powerups.DefaultBulletBuff;
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
        #endregion Property Field

        #region Prefabs

        #endregion Prefabs


        #region Prefab Properties

        #endregion Prefab Properties


        #region Penetration

        private int NumberPenetrated { get; set; }
        public bool CanPenetrate => NumberPenetrated == 0
            && DefaultBulletBuffs.BulletPenetrationChance > 0;
        protected sealed override bool AutomaticallyDeactivate => !CanPenetrate || !RandomUtil.Bool(DefaultBulletBuffs.BulletPenetrationChance);

        #endregion Penetration

        protected float InitialScale { get; private set; }


        protected virtual void OnDefaultInfluencedBulletInit() { }
        protected sealed override void OnPlayerBulletInit()
        {
            InitialScale = LocalScale;
            OnDefaultInfluencedBulletInit();
        }

        protected virtual void OnDefaultInfluencedBulletActivate() { }
        protected sealed override void OnActivate()
        {
            NumberPenetrated = 0;
            LocalScale = CalculateLocalScale();
            EnemyParticlesScale = CalculateParticlesScale();
            OnDefaultInfluencedBulletActivate();
        }

        protected virtual void OnDefaultInfluencedBulletCollideWithEnemy(Enemy enemy, Vector3 hitPosition) { }
        protected sealed override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            OnDefaultInfluencedBulletCollideWithEnemy(enemy, hitPosition);
            NumberPenetrated++;
        }









        private float CalculateLocalScale()
        {
            float scaleIncrease = 1f;

            scaleIncrease += DefaultBulletBuffs.SizeScaleIncrease;

            float localScale = InitialScale * scaleIncrease;
            return localScale;
        }

        protected Vector2 CalculateVelocity(float initialSpeed) => CalculateVelocity(new Vector2(0, initialSpeed));
        protected Vector2 CalculateVelocity(Vector2 velocity)
        {
            float scale = CalculateVelocityScale();
            velocity *= scale;

            return velocity;
        }

        protected float CalculateVelocityScale()
        {
            float scaleIncrease = 1f;

            scaleIncrease += DefaultBulletBuffs.SpeedScaleIncrease;

            return scaleIncrease;
        }

        private float CalculateParticlesScale()
        {
            float scale = 1f;

            scale += DefaultBulletBuffs.ParticlesScaleIncrease;

            return scale;
        }

        protected int CalculateDamage()
        {
            int damage = BaseDamage;


            #region Scale

            float scaleIncrease = 1f;

            scaleIncrease += DefaultBulletBuffs.DamageScaleIncrease;

            damage = (int)(damage * scaleIncrease);

            #endregion Scale

            #region Flat Increase

            //damage += SnakeBiteDamage;

            #endregion Flat Increase

            return damage;
        }
    }
}