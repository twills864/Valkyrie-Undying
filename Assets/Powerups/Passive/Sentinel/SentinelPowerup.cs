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

        protected override LevelValueCalculator InitialValueCalculator
            => new ProductLevelValueCalculator(RespawnIntervalBase, RespawnIntervalRatioIncrease);

        public float RespawnInterval => ValueCalculator.Value;

        private const float WorldDistanceExponentBase = 0.6f;
        private const float WorldDistanceMaxValue = 2.5f;
        private AsymptoteScaleLevelValueCalculator DistanceCalculator =
            new AsymptoteScaleLevelValueCalculator(WorldDistanceExponentBase, WorldDistanceMaxValue);

        public float Radius => DistanceCalculator.Value;

        public override void OnLevelUp()
        {
            //if(Level == 1)
            //    RainCloudSpawner.Instance.Activate();

            DistanceCalculator.Level = Level;
            SentinelManager.Instance.LevelUp(Level, Radius, RespawnInterval);
        }

        public override void RunFrame(float deltaTime)
        {
            SentinelManager.Instance.transform.position = Player.Instance.transform.position;
            SentinelManager.Instance.RunFrame(deltaTime);
        }
    }
}

