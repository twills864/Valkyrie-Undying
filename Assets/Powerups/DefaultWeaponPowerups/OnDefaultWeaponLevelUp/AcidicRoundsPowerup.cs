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
    ///
    /// </summary>
    /// <inheritdoc/>
    public class AcidicRoundsPowerup : OnDefaultWeaponLevelUpPowerup
    {
        protected override Sprite GetPowerupSprite(PowerupSpriteBank bank) => bank.AcidicRounds;

        public int AcidDamage => (int) AcidDamageCalculator.Value;
        private SumLevelValueCalculator AcidDamageCalculator { get; set; }

        protected override void InitBalance(in PowerupBalanceManager.OnDefaultWeaponLevelUpBalance balance)
        {
            float damageBase = balance.AcidicRounds.AcidDamage.Base;
            float damageIncrease = balance.AcidicRounds.AcidDamage.Increase;
            AcidDamageCalculator = new SumLevelValueCalculator(damageBase, damageIncrease);
        }

        public override void OnLevelUp()
        {
            DefaultBulletBuffs.PiercingRoundsLevelup(this);
        }
    }
}
