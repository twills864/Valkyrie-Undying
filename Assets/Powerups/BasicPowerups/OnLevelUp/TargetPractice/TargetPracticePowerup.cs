using System;
using System.Linq;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.UI.SpriteBank;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Spawns a target that explodes into several bullets after being
    /// shot by the player. The bullets that shoot the target
    /// are not consumed in this process.
    /// </summary>
    /// <inheritdoc/>
    [Obsolete(ObsoleteConstants.NotProductionReady)]
    public class TargetPracticePowerup : OnLevelUpPowerup
    {
        protected override Sprite GetPowerupSprite(PowerupSpriteBank bank) => bank.TargetPractice;

        private float PowerValue => PowerCalculator.Value;
        private SumLevelValueCalculator PowerCalculator { get; set; }

        protected override void InitBalance(in PowerupBalanceManager.OnLevelUpBalance balance)
        {
            float powerBase = balance.TargetPractice.Power.Base;
            float powerIncrease = balance.TargetPractice.Power.Increase;
            PowerCalculator = new SumLevelValueCalculator(powerBase, powerIncrease);
        }

        public override void OnLevelUp()
        {
            GameManager.Instance.SpawnTargetPractice(this);
        }
    }
}
