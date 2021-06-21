using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.Powerups.DefaultBulletBuff;
using Assets.UnityPrefabStructs;
using UnityEngine;

namespace Assets.Powerups.DefaultBulletBuff
{
    public static class DefaultBulletBuffs
    {
        #region Property Fields
        private static FireDamage _FireDamage = default;
        #endregion Property Field

        public static float DamageScaleIncrease => DamageScaleIncreaseTotal.Total;
        public static float SizeScaleIncrease => AugmentedRounds.DamageScaleIncrease;
        public static float SpeedScaleIncrease => AugmentedRounds.SpeedScaleIncrease;
        public static float ParticlesScaleIncrease => AugmentedRounds.ParticlesScaleIncrease;

        public static int BulletPenetration { get; private set; }

        private static PosionDamageTotal PoisonDamage;
        private static ParasiteDamageTotal ParasiteDamage;

        #region FireDamage

        public static bool IsBurning => FireCollisionDamage > 0;

        public static int FireCollisionDamage
        {
            get => _FireDamage.CollisionDamage;
            private set => _FireDamage.CollisionDamage = value;
        }

        public static int FireDamageIncreasePerTick
        {
            get => _FireDamage.DamageIncreasePerTick;
            private set => _FireDamage.DamageIncreasePerTick = value;
        }

        public static int FireMaxDamage
        {
            get => _FireDamage.MaxDamage;
            private set => _FireDamage.MaxDamage = value;
        }

        #endregion FireDamage

        private static AcidDamageTotal AcidDamage;
        private static ChilledTimeTotal ChilledTime;
        private static SilenceTimeTotal SilenceTime;

        private static DamageScaleIncreaseTotal DamageScaleIncreaseTotal;

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

        #endregion Augmented Rounds

        public static void Init()
        {
            BulletPenetration = 0;

            PoisonDamage.Reset();
            ParasiteDamage.Reset();
            _FireDamage.Reset();
            AcidDamage.Reset();
            ChilledTime.Reset();
            SilenceTime.Reset();

            DamageScaleIncreaseTotal.Reset();

            AugmentedRounds.Reset();
        }

        /// <summary>
        /// Halves a given <paramref name="damage"/>, rounding up.
        /// Assumes that the input damage is greater than 0.
        /// </summary>
        /// <param name="damage">The damage to halve.</param>
        private static int HalveBonusDamage(int damage) => (damage + 1) / 2;

        public static void OnDefaultBulletHit(DefaultBullet bullet, Enemy enemy, Vector3 hitPosition)
        {
            Func<int, int> scale = x => x;
            OnCollideWithEnemy(bullet, enemy, hitPosition, scale);
        }

        public static void OnDefaultExtraBulletHit(DefaultExtraBullet bullet, Enemy enemy, Vector3 hitPosition)
        {
            OnCollideWithEnemy(bullet, enemy, hitPosition, HalveBonusDamage);
        }

        public static void OnOthelloBulletHit(OthelloBullet bullet, Enemy enemy, Vector3 hitPosition)
        {
            OnCollideWithEnemy(bullet, enemy, hitPosition, HalveBonusDamage);
        }

        private static void OnCollideWithEnemy(PlayerBullet bullet, Enemy enemy, Vector3 hitPosition, Func<int, int> ScaleDamage)
        {
            if (enemy.isActiveAndEnabled)
                ApplyStatus(enemy, ScaleDamage);
        }

        private static void ApplyStatus(Enemy enemy, Func<int, int> ScaleDamage)
        {
            if (PoisonDamage.Total > 0)
                enemy.AddPoison(ScaleDamage(PoisonDamage.Total));

            if (ParasiteDamage.Total > 0)
                enemy.AddParasites(ScaleDamage(ParasiteDamage.Total));

            if (IsBurning)
                enemy.Ignite(ScaleDamage(FireCollisionDamage), ScaleDamage(FireDamageIncreasePerTick), FireMaxDamage);

            if (AcidDamage.Total > 0)
                enemy.AddAcid(ScaleDamage(AcidDamage.Total));

            if (ChilledTime.Total > 0)
                enemy.AddChill(ScaleDamage(ChilledTime.Total));

            //if (SilenceTime.Total > 0)
            //    enemy.Silence(ScaleDamage(SilenceTime.Total));
        }


        #region Level Up

        public static void AugmentedRoundsLevelUp(AugmentedRoundsPowerup powerup)
        {
            AugmentedRounds.DamageScaleIncrease = powerup.DamageScaleIncrease;
            DamageScaleIncreaseTotal.AugmentedRoundsIncrease = powerup.DamageScaleIncrease;
            AugmentedRounds.SizeScaleIncrease = powerup.SizeScaleIncrease;
            AugmentedRounds.SpeedScaleIncrease = powerup.SpeedScaleIncrease;
            AugmentedRounds.ParticlesScaleIncrease = powerup.ParticlesScaleIncrease;
        }

        public static void PiercingRoundsLevelup(PiercingRoundsPowerup powerup)
        {
            BulletPenetration = 1;
        }

        public static void InfestedRoundsLevelup(InfestedRoundsPowerup powerup)
        {
            ParasiteDamage.InfestedRoundsDamage = powerup.NumParasites;
        }

        public static void SnakeBiteLevelUp(SnakeBitePowerup powerup)
        {
            PoisonDamage.SnakeBiteDamage = powerup.PoisonDamage;
        }

        public static void VenomousRoundsLevelup(VenomousRoundsPowerup powerup)
        {
            PoisonDamage.VenomousRoundsDamage = powerup.PoisonDamage;
        }

        public static void CryogenicRoundsLevelup(CryogenicRoundsPowerup powerup)
        {
            ChilledTime.CryogenicRoundsTime = powerup.ChillTime;
            DamageScaleIncreaseTotal.CryogenicRoundsIncrease = powerup.DamageScaleIncrease;
        }

        public static void PiercingRoundsLevelup(AcidicRoundsPowerup powerup)
        {
            AcidDamage.AcidicRoundsDamage = powerup.AcidDamage;
        }

        #endregion
    }
}
