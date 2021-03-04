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
    /// Spawns a damaging void at the player's postion on getting hit.
    /// </summary>
    /// <inheritdoc/>
    public class RetributionPowerup : OnGetHitPowerup
    {
        private const float DurationBase = 0.7f;
        private const float DurationMax = 3.0f;

        private const float SizeScaleInitialValue = 0.4f;
        private const float SizeScaleExponentBase = 0.7f;
        private const float SizeScaleMax = 15f;

        protected override LevelValueCalculator InitialValueCalculator
            => new AsymptoteScaleLevelValueCalculator(DurationBase, DurationMax);

        private float Duration => ValueCalculator.Value;

        private AsymptoteRatioLevelValueCalculator SizeScaleLevel =
            new AsymptoteRatioLevelValueCalculator(SizeScaleInitialValue, SizeScaleExponentBase, SizeScaleMax);

        private float SizeScale => SizeScaleLevel.Value;

        public override void OnLevelUp()
        {
            SizeScaleLevel.Level = Level;
        }

        public override void OnGetHit()
        {
            var position = Player.Instance.ColliderMap.Center;
            RetributionBullet.StartRetribution(position, Level, SizeScale, Duration);
        }
    }
}
