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
    /// Adds chill time to each enemy hit.
    /// </summary>
    /// <inheritdoc/>
    [Obsolete(ObsoleteConstants.FollowTheFun)]
    public class CryogenicRoundsPowerup : OnDefaultWeaponLevelUpPowerup
    {
        public override int MaxLevel => 1;
        protected override Sprite GetPowerupSprite(PowerupSpriteBank bank) => bank.CryogenicRounds;

        public int ChillTime => (int)ChillTimeCalculator.Value;
        private SumLevelValueCalculator ChillTimeCalculator { get; set; }

        public float DamageScaleIncrease => DamageScaleIncreaseCalculator.Value;
        private SumLevelValueCalculator DamageScaleIncreaseCalculator { get; set; }

        protected override void InitBalance(in PowerupBalanceManager.OnDefaultWeaponLevelUpBalance balance)
        {
            float chillTimeBase = balance.CryogenicRounds.ChillTime.Base;
            float chillTimeIncrease = balance.CryogenicRounds.ChillTime.Increase;
            ChillTimeCalculator = new SumLevelValueCalculator(chillTimeBase, chillTimeIncrease);

            float damageScaleIncreaseBase = balance.CryogenicRounds.DamageScaleIncrease.Base;
            float damageScaleIncreaseIncrease = balance.CryogenicRounds.DamageScaleIncrease.Increase;
            DamageScaleIncreaseCalculator = new SumLevelValueCalculator(damageScaleIncreaseBase, damageScaleIncreaseIncrease);
        }

        public override void OnLevelUp()
        {
            DefaultBulletBuffs.CryogenicRoundsLevelup(this);
        }
    }
}
