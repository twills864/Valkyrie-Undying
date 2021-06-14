using System;
using System.Linq;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.FireStrategies.PlayerFireStrategies;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.UI.SpriteBank;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Adds 5 poison damage to the player's default bullets.
    /// </summary>
    /// <inheritdoc/>
    public class VenomousRoundsPowerup : OnDefaultWeaponFirePowerup
    {
        protected override Sprite GetPowerupSprite(PowerupSpriteBank bank) => bank.SnakeBite;

        public int PoisonDamage => (int) PoisonDamageCalculator.Value;
        private SumLevelValueCalculator PoisonDamageCalculator { get; set; }

        protected override void InitBalance(in PowerupBalanceManager.OnDefaultWeaponFireBalance balance)
        {
            float poisonDamageBase = balance.VenomousRounds.PoisonDamage.Base;
            float poisonDamageIncrease = balance.VenomousRounds.PoisonDamage.Increase;
            PoisonDamageCalculator = new SumLevelValueCalculator(poisonDamageBase, poisonDamageIncrease);
        }

        public override void OnFire(Vector3 position, DefaultBullet[] bullets)
        {
            foreach (var bullet in bullets)
                bullet.VenomousRoundsDamage += PoisonDamage;
        }
    }
}
