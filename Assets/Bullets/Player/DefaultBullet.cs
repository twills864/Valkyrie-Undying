﻿using System;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.Powerups;
using Assets.ScreenEdgeColliders;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class DefaultBullet : PlayerBullet
    {
        public override int Damage => CalculateDamage();
        public int ExtraBulletDamage => Damage / 2;

        public override AudioClip FireSound => SoundBank.LaserBasic;

        public static void StaticInit()
        {
            MaxPenetration = 0;

            ReboundActive = false;

            AugmentedRounds.Reset();
        }

        #region Prefabs

        [SerializeField]
        private float _InitialSpeed = GameConstants.PrefabNumber;

        [SerializeField]
        private float _DefaultExtraBulletScaleRatio = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public float InitialSpeed => _InitialSpeed;
        public float DefaultExtraBulletScaleRatio => _DefaultExtraBulletScaleRatio;

        #endregion Prefab Properties


        #region Penetration

        public static int MaxPenetration { get; set; }
        public int NumberPenetrated { get; private set; }

        protected override bool AutomaticallyDeactivate => NumberPenetrated >= MaxPenetration;

        #endregion Penetration

        public static bool ReboundActive { get; set; } = false;

        #region Augmented Rounds

        private struct AugmentedRoundsScaling
        {
            public float DamageScaleIncrease;
            public float SizeScaleIncrease;
            public float SpeedScaleIncrease;
            public float ParticlesScaleIncrease;

            public void Reset()
            {
                DamageScaleIncrease = 0f;
                SizeScaleIncrease = 0f;
                SpeedScaleIncrease = 0f;
                ParticlesScaleIncrease = 0f;
            }
        }

        private static AugmentedRoundsScaling AugmentedRounds;

        public static void AugmentedRoundsLevelUp(AugmentedRoundsPowerup powerup)
        {
            AugmentedRounds.DamageScaleIncrease = powerup.DamageScaleIncrease;
            AugmentedRounds.SizeScaleIncrease = powerup.SizeScaleIncrease;
            AugmentedRounds.SpeedScaleIncrease = powerup.SpeedScaleIncrease;
            AugmentedRounds.ParticlesScaleIncrease = powerup.ParticlesScaleIncrease;
        }

        private float InitialScale { get; set; }

        #endregion Augmented Rounds

        public int PoisonDamage => SnakeBiteDamage + VenomousRoundsDamage;

        [Obsolete(ObsoleteConstants.FollowTheFun)]
        public int SnakeBiteDamage { get; set; }
        public int VenomousRoundsDamage { get; set; }

        public int ParasiteDamage { get; set; }

        protected override void OnPlayerBulletInit()
        {
            InitialScale = LocalScale;
        }

        protected override void OnActivate()
        {
            LocalScale = CalculateLocalScale();
            EnemyParticlesScale = CalculateParticlesScale();
            NumberPenetrated = 0;
            SnakeBiteDamage = 0;
            VenomousRoundsDamage = 0;
            ParasiteDamage = 0;
        }

        protected override void OnBulletSpawn()
        {
            Velocity = CalculateVelocity();
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {

        }

        protected override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            GameManager.Instance.OnEnemyHitWithDefaultWeapon(enemy, this, hitPosition);

            if(PoisonDamage > 0)
                enemy.AddPoison(PoisonDamage);

            if(ParasiteDamage > 0)
                enemy.AddParasites(ParasiteDamage);

            NumberPenetrated++;
        }

        protected override void OnPlayerBulletTriggerEnter2D(Collider2D collision)
        {
            if(ReboundActive && CollisionUtil.IsScreenEdge(collision, out ScreenSide screenSide)
                && screenSide == ScreenSide.Top)
            {
                ReboundPowerup.ReboundOffScreenEdge(this);
            }
        }

        private int CalculateDamage()
        {
            int damage = BaseDamage;


            #region Scale

            float scaleIncrease = 1f;

            scaleIncrease += AugmentedRounds.DamageScaleIncrease;

            damage = (int)(damage * scaleIncrease);

            #endregion Scale

            #region Flat Increase

            //damage += SnakeBiteDamage;

            #endregion Flat Increase

            return damage;
        }

        private float CalculateLocalScale()
        {
            float scaleIncrease = 1f;

            scaleIncrease += AugmentedRounds.SizeScaleIncrease;

            float localScale = InitialScale * scaleIncrease;
            return localScale;
        }

        private Vector2 CalculateVelocity()
        {
            Vector2 velocity = new Vector2(0, InitialSpeed);
            float scale = CalculateVelocityScale();
            velocity *= scale;

            return velocity;
        }

        public float CalculateVelocityScale()
        {
            float scaleIncrease = 1f;

            scaleIncrease += AugmentedRounds.SpeedScaleIncrease;

            return scaleIncrease;
        }

        private float CalculateParticlesScale()
        {
            float scale = 1f;

            scale += AugmentedRounds.ParticlesScaleIncrease;

            return scale;
        }
    }
}