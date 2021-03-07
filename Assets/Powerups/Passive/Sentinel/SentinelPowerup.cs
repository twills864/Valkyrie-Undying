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
    public class SentinelPowerup : PassivePowerup
    {
        protected override void InitBalance(in PowerupBalanceManager.PassiveBalance balance)
        {
            float respawnIntervalBase = balance.Sentinel.RespawnInterval.Base;
            float respawnIntervalRatioIncrease = balance.Sentinel.RespawnInterval.Increase;
            RespawnIntervalCalculator = new ProductLevelValueCalculator(respawnIntervalBase, respawnIntervalRatioIncrease);

            float worldDistanceExponentBase = balance.Sentinel.WorldDistance.Base;
            float worldDistanceMaxValue = balance.Sentinel.WorldDistance.MaxValue;
            DistanceCalculator = new AsymptoteScaleLevelValueCalculator(worldDistanceExponentBase, worldDistanceMaxValue);
        }

        public float RespawnInterval => RespawnIntervalCalculator.Value;
        private ProductLevelValueCalculator RespawnIntervalCalculator { get; set; }

        public float Radius => DistanceCalculator.Value;
        private AsymptoteScaleLevelValueCalculator DistanceCalculator { get; set; }

        public override void OnLevelUp()
        {
            SentinelManager.Instance.LevelUp(Level, Radius, RespawnInterval);
        }

        public override void RunFrame(float deltaTime)
        {
            SentinelManager.Instance.transform.position = Player.Instance.transform.position;
            SentinelManager.Instance.RunFrame(deltaTime);
        }
    }
}

