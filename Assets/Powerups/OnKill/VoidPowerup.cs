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
        private const float DurationMax = 2.0f;

        private const float SizeScaleBase = 0.7f;
        private const float SizeScaleMax = 0.15f;

        protected override LevelValueCalculator InitialValueCalculator
            => new AsymptoteScaleLevelValueCalculator(DurationBase, DurationMax);

        private float Duration => ValueCalculator.Value;

        private AsymptoteScaleLevelValueCalculator SizeScaleLevel =
            new AsymptoteScaleLevelValueCalculator(SizeScaleBase, SizeScaleMax);

        private float SizeScale => SizeScaleLevel.Value;

        public override void OnLevelUp()
        {
            SizeScaleLevel.Level = Level;
        }

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
