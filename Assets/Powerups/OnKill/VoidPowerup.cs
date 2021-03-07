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
    /// Opens a void on killing an enemy.
    /// </summary>
    /// <inheritdoc/>
    public class VoidPowerup : OnKillPowerup
    {
        protected override void InitBalance(in PowerupBalanceManager.OnKillBalance balance)
        {
            float durationBase = balance.Void.Duration.Base;
            float durationMax = balance.Void.Duration.Max;
            DurationCalculator = new AsymptoteScaleLevelValueCalculator(durationBase, durationMax);

            float sizeScaleBase = balance.Void.SizeScale.Base;
            float sizeScaleMax = balance.Void.SizeScale.Max;
            SizeScaleLevel = new AsymptoteScaleLevelValueCalculator(sizeScaleBase, sizeScaleMax);
        }

        private float Duration => DurationCalculator.Value;
        private AsymptoteScaleLevelValueCalculator DurationCalculator { get; set; }

        private float SizeScale => SizeScaleLevel.Value;
        private AsymptoteScaleLevelValueCalculator SizeScaleLevel { get; set; }

        public override void OnKill(Enemy enemy, PlayerBullet bullet)
        {
            if (enemy.CanVoidPause)
            {
                var position = enemy.ColliderMap.Center;
                VoidBullet.StartVoid(position, Level, SizeScale, Duration);
            }
        }
    }
}
