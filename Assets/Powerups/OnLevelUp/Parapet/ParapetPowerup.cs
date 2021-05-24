using System;
using System.Linq;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    ///
    /// </summary>
    /// <inheritdoc/>
    public class ParapetPowerup : OnLevelUpPowerup
    {
        public override int MaxLevel => 1;

        private float Height => HeightCalculator.Value;
        private SumLevelValueCalculator HeightCalculator { get; set; }

        private Vector2 Scale => new Vector2(ScaleXCalculator.Value, ScaleYCalculator.Value);
        private SumLevelValueCalculator ScaleXCalculator { get; set; }
        private SumLevelValueCalculator ScaleYCalculator { get; set; }

        protected override void InitBalance(in PowerupBalanceManager.OnLevelUpBalance balance)
        {
            float heightBase = balance.Parapet.Height.Base;
            float heightIncrease = balance.Parapet.Height.IncreasePerLevel;
            HeightCalculator = new SumLevelValueCalculator(heightBase, heightIncrease);

            Vector2 scaleBase = balance.Parapet.Scale.Base;
            Vector2 scaleIncrease = balance.Parapet.Scale.IncreasePerLevel;
            ScaleXCalculator = new SumLevelValueCalculator(scaleBase.x, scaleIncrease.x);
            ScaleYCalculator = new SumLevelValueCalculator(scaleBase.y, scaleIncrease.y);
        }

        public override void OnLevelUp()
        {
            Player.Instance.ActivateParapets(Height, Scale);
        }
    }
}
