using System;
using System.Linq;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.Powerups.DefaultBulletBuff;
using Assets.UI.SpriteBank;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Increases the size, damage, and speed of the player's default bullets.
    /// </summary>
    /// <inheritdoc/>
    public class AugmentedRoundsPowerup : OnDefaultWeaponLevelUpPowerup
    {
        public override int MaxLevel => 1;
        protected override Sprite GetPowerupSprite(PowerupSpriteBank bank) => bank.AugmentedRounds;

        public float SizeScaleIncrease => SizeIncreaseCalculator.Value;
        private SumLevelValueCalculator SizeIncreaseCalculator { get; set; }

        public float DamageScaleIncrease => DamageIncreaseCalculator.Value;
        private SumLevelValueCalculator DamageIncreaseCalculator { get; set; }

        public float SpeedScaleIncrease => SpeedIncreaseCalculator.Value;
        private SumLevelValueCalculator SpeedIncreaseCalculator { get; set; }

        public float ParticlesScaleIncrease => ParticlesIncreaseCalculator.Value;
        private SumLevelValueCalculator ParticlesIncreaseCalculator { get; set; }

        protected override void InitBalance(in PowerupBalanceManager.OnDefaultWeaponLevelUpBalance balance)
        {
            float sizeBase = balance.AugmentedRounds.Size.Base;
            float sizeIncrease = balance.AugmentedRounds.Size.Increase;
            SizeIncreaseCalculator = new SumLevelValueCalculator(sizeBase, sizeIncrease);

            float damageBase = balance.AugmentedRounds.Damage.Base;
            float damageIncrease = balance.AugmentedRounds.Damage.Increase;
            DamageIncreaseCalculator = new SumLevelValueCalculator(damageBase, damageIncrease);

            float speedBase = balance.AugmentedRounds.Speed.Base;
            float speedIncrease = balance.AugmentedRounds.Speed.Increase;
            SpeedIncreaseCalculator = new SumLevelValueCalculator(speedBase, speedIncrease);

            float particlesBase = balance.AugmentedRounds.Particles.Base;
            float particlesIncrease = balance.AugmentedRounds.Particles.Increase;
            ParticlesIncreaseCalculator = new SumLevelValueCalculator(particlesBase, particlesIncrease);
        }

        public override void OnLevelUp()
        {
            DefaultBulletBuffs.AugmentedRoundsLevelUp(this);
        }
    }
}
