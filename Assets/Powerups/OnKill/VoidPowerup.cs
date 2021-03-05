using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Util;

namespace Assets.Powerups
{
    /// <summary>
    /// Opens a void on killing an enemy.
    /// </summary>
    /// <inheritdoc/>
    public class VoidPowerup : OnKillPowerup
    {
        private const float DurationBase = 0.7f;
        private const float DurationMax = 3.0f;

        private const float SizeScaleBase = 0.7f;
        private const float SizeScaleMax = 3f;

        private float Duration => DurationCalculator.Value;
        private AsymptoteScaleLevelValueCalculator DurationCalculator { get; set; }
            = new AsymptoteScaleLevelValueCalculator(DurationBase, DurationMax);

        private float SizeScale => SizeScaleLevel.Value;
        private AsymptoteScaleLevelValueCalculator SizeScaleLevel { get; set; }
            = new AsymptoteScaleLevelValueCalculator(SizeScaleBase, SizeScaleMax);

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
