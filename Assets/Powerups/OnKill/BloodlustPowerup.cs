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

        protected override LevelValueCalculator InitialValueCalculator
            => new SumLevelValueCalculator(BaseDuration, DurationIncrease);

        private float Duration => ValueCalculator.Value;

        private SumLevelValueCalculator SpeedScaleLevel =
            new SumLevelValueCalculator(BaseSpeedScale, SpeedScaleIncrease);

        private float SpeedScale => SpeedScaleLevel.Value;

        public override void OnLevelUp()
        {
            SpeedScaleLevel.Level = Level;
        }

        public override void OnKill(Enemy enemy, PlayerBullet bullet)
        {
            GameManager.Instance.SetBloodlust(Duration, SpeedScale);
        }
    }
}
