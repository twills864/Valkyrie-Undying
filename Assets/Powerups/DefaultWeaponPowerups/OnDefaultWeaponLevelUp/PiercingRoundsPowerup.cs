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
    /// Piercing Rounds give default bullets a chance to penetrate enemies.
    /// Extra bullets can also receive this effect.
    /// </summary>
    /// <inheritdoc/>
    public class PiercingRoundsPowerup : OnDefaultWeaponLevelUpPowerup
    {
        public override int MaxLevel => 1;
        protected override Sprite GetPowerupSprite(PowerupSpriteBank bank) => bank.PiercingRounds;

        public float PierceChance => PierceChanceCalculator.Value;
        private SumLevelValueCalculator PierceChanceCalculator { get; set; }

        protected override void InitBalance(in PowerupBalanceManager.OnDefaultWeaponLevelUpBalance balance)
        {
            float pierceChanceBase = balance.PiercingRounds.PierceChance.Base;
            float pierceChanceIncrease = balance.PiercingRounds.PierceChance.Increase;
            PierceChanceCalculator = new SumLevelValueCalculator(pierceChanceBase, pierceChanceIncrease);
        }

        public override void OnLevelUp()
        {
            DefaultBulletBuffs.PiercingRoundsLevelup(this);
        }
    }
}
