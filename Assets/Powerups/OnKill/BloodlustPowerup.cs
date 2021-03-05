using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.Util;

namespace Assets.Powerups
{
    /// <summary>
    /// Gives a brief attack speed boost on killing an enemy.
    /// </summary>
    /// <inheritdoc/>
    public class BloodlustPowerup : OnKillPowerup
    {
        private const float BaseDuration = 0.6f;
        private const float DurationIncrease = 0.3f;

        private const float BaseSpeedScale = 1.3f;
        private const float SpeedScaleIncrease = 0.15f;

        private float Duration => DurationCalculator.Value;
        private SumLevelValueCalculator DurationCalculator { get; }
            = new SumLevelValueCalculator(BaseDuration, DurationIncrease);

        private float SpeedScale => SpeedScaleLevel.Value;
        private SumLevelValueCalculator SpeedScaleLevel { get; }
            = new SumLevelValueCalculator(BaseSpeedScale, SpeedScaleIncrease);

        public override void OnKill(Enemy enemy, PlayerBullet bullet)
        {
            GameManager.Instance.SetBloodlust(Duration, SpeedScale);
        }
    }
}
