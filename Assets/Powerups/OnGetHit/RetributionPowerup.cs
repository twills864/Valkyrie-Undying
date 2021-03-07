using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.Util;

namespace Assets.Powerups
{
    /// <summary>
    /// Spawns a damaging void at the player's position on getting hit.
    /// </summary>
    /// <inheritdoc/>
    public class RetributionPowerup : OnGetHitPowerup
    {
        protected override void InitBalance(in PowerupBalanceManager.OnGetHitBalance balance)
        {
            float durationBase = balance.Retribution.Duration.Base;
            float durationMax = balance.Retribution.Duration.Max;
            DurationCalculator = new AsymptoteScaleLevelValueCalculator(durationBase, durationMax);

            float sizeScaleInitialValue = balance.Retribution.SizeScale.InitialValue;
            float sizeScaleExponentBase = balance.Retribution.SizeScale.Base;
            float sizeScaleMax = balance.Retribution.SizeScale.Max;
            SizeScaleLevel = new AsymptoteRatioLevelValueCalculator
                (sizeScaleInitialValue, sizeScaleExponentBase, sizeScaleMax);
        }

        private float Duration => DurationCalculator.Value;
        private AsymptoteScaleLevelValueCalculator DurationCalculator { get; set; }

        private float SizeScale => SizeScaleLevel.Value;
        private AsymptoteRatioLevelValueCalculator SizeScaleLevel { get; set; }

        public override void OnGetHit()
        {
            var position = Player.Instance.ColliderMap.Center;
            RetributionBullet.StartRetribution(position, Level, SizeScale, Duration);
        }


    }
}
