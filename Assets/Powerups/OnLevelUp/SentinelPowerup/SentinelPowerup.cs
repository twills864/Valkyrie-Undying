using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets.Powerups
{
    /// <summary>
    /// Summons eight bullets that revolve around the player, respawning periodically.
    /// </summary>
    /// <inheritdoc/>
    public class SentinelPowerup : OnLevelUpPowerup
    {
        private const float RespawnIntervalBase = 2.0f;
        private const float RespawnIntervalRatioIncrease = 0.9f;

        protected override LevelValueCalculator InitialValueCalculator
            => new ProductLevelValueCalculator(RespawnIntervalBase, RespawnIntervalRatioIncrease);

        public float RespawnInterval => ValueCalculator.Value;

        private const float WorldDistanceExponentBase = 0.6f;
        private const float WorldDistanceMaxValue = 2f;
        private LevelValueCalculator DistanceCalculator =
            new AsymptoteScaleLevelValueCalculator(WorldDistanceExponentBase, WorldDistanceMaxValue);

        public float Distance => DistanceCalculator.Value;

        public override void OnLevelUp()
        {
            //if(Level == 1)
            //    RainCloudSpawner.Instance.Activate();

            DistanceCalculator.Level = Level;
            SentinelManager.Instance.LevelUp(Level, Distance, RespawnInterval);
        }
    }
}

