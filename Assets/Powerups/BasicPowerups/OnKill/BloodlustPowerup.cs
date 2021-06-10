using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.Powerups.Balance;
using Assets.UI.SpriteBank;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Gives a brief attack speed boost on killing an enemy.
    /// </summary>
    /// <inheritdoc/>
    public class BloodlustPowerup : OnKillPowerup
    {
        public override int MaxLevel => 2;
        protected override Sprite GetPowerupSprite(PowerupSpriteBank bank) => bank.Bloodlust;

        private float Duration => DurationCalculator.Value;
        private SumLevelValueCalculator DurationCalculator { get; set; }

        private float SpeedScale => SpeedScaleLevel.Value;
        private SumLevelValueCalculator SpeedScaleLevel { get; set; }

        protected override void InitBalance(in PowerupBalanceManager.OnKillBalance balance)
        {
            float baseDuration = balance.Bloodlust.Duration.Base;
            float durationIncrease = balance.Bloodlust.Duration.IncreasePerLevel;
            DurationCalculator = new SumLevelValueCalculator(baseDuration, durationIncrease);

            float baseSpeedScale = balance.Bloodlust.SpeedScale.Base;
            float speedScaleIncrease = balance.Bloodlust.SpeedScale.Increase;
            SpeedScaleLevel = new SumLevelValueCalculator(baseSpeedScale, speedScaleIncrease);
        }

        private bool ResetFireStrategy => Level > 1;

        public override void OnLevelUp()
        {
            ActivateBloodlust();
        }

        public override void OnKill(Enemy enemy, PlayerBullet bullet)
        {
            ActivateBloodlust();
        }

        private void ActivateBloodlust()
        {
            GameManager.Instance.SetBloodlust(Duration, SpeedScale, ResetFireStrategy);
        }
    }
}
