﻿using System;
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
            float durationIncrease = balance.Retribution.Duration.Increase;
            DurationCalculator = new SumLevelValueCalculator(durationBase, durationIncrease);

            float sizeScaleBase = balance.Retribution.SizeScale.Base;
            float sizeScaleIncrease = balance.Retribution.SizeScale.Increase;
            SizeScaleLevel = new SumLevelValueCalculator(sizeScaleBase, sizeScaleIncrease);
        }

        private float Duration => DurationCalculator.Value;
        private SumLevelValueCalculator DurationCalculator { get; set; }

        private float SizeScale => SizeScaleLevel.Value;
        private SumLevelValueCalculator SizeScaleLevel { get; set; }

        public override void OnGetHit()
        {
            var position = Player.Instance.ColliderMap.Center;
            RetributionBullet.StartRetribution(position, Level, SizeScale, Duration);
        }


    }
}
