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
    public class SentinelPowerup : PassivePowerup
    {
        private const float RespawnIntervalBase = 5.0f;
        private const float RespawnIntervalRatioIncrease = 0.75f;

        private const float WorldDistanceExponentBase = 0.6f;
        private const float WorldDistanceMaxValue = 2.5f;

        public float RespawnInterval => RespawnIntervalCalculator.Value;
        private ProductLevelValueCalculator RespawnIntervalCalculator { get; }
            = new ProductLevelValueCalculator(RespawnIntervalBase, RespawnIntervalRatioIncrease);

        public float Radius => DistanceCalculator.Value;
        private AsymptoteScaleLevelValueCalculator DistanceCalculator { get; }
            = new AsymptoteScaleLevelValueCalculator(WorldDistanceExponentBase, WorldDistanceMaxValue);

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

