using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Powerups.Balance;
using Assets.Util;

namespace Assets.Powerups
{
    /// <summary>
    /// Summons eight bullets that revolve around the player, respawning periodically.
    /// </summary>
    /// <inheritdoc/>
    public class SentinelPowerup : OnLevelUpPowerup
    {
        protected override void InitBalance(in PowerupBalanceManager.OnLevelUpBalance balance)
        {
            float respawnIntervalBase = balance.Sentinel.RespawnInterval.Base;
            float respawnIntervalScalePerLevel = balance.Sentinel.RespawnInterval.ScalePerLevel;
            RespawnIntervalCalculator = new ProductLevelValueCalculator(respawnIntervalBase, respawnIntervalScalePerLevel);

            Radius = balance.Sentinel.Radius;

            //float worldDistanceExponentBase = balance.Sentinel.WorldDistance.Base;
            //float worldDistanceMaxValue = balance.Sentinel.WorldDistance.MaxValue;
            //DistanceCalculator = new AsymptoteScaleLevelValueCalculator(worldDistanceExponentBase, worldDistanceMaxValue);
        }

        public float RespawnInterval => RespawnIntervalCalculator.Value;
        private ProductLevelValueCalculator RespawnIntervalCalculator { get; set; }

        public float Radius { get; set; }
        //private AsymptoteScaleLevelValueCalculator DistanceCalculator { get; set; }

        public override void OnLevelUp()
        {
            SentinelManager.Instance.LevelUp(Level, Radius, RespawnInterval);
        }
    }
}

