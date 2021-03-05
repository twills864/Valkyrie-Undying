using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets.Powerups
{
    /// <summary>
    /// Spawns a passive rain cloud behind the player that rains bullets on enemies.
    /// </summary>
    /// <inheritdoc/>
    public class RainCloudPowerup : PassivePowerup
    {
        private const float FireSpeedBase = 0.5f;
        private const float FireSpeedIncrease = 0.9f;

        private const float DamageBase = 10;
        private const float DamageIncrease = 1;

        public float FireSpeed => FireSpeedCalculator.Value;
        private ProductLevelValueCalculator FireSpeedCalculator { get; }
            = new ProductLevelValueCalculator(FireSpeedBase, FireSpeedIncrease);

        public int Damage => (int) DamageCalculator.Value;
        private SumLevelValueCalculator DamageCalculator { get; }
            = new SumLevelValueCalculator(DamageBase, DamageIncrease);

        public override void OnLevelUp()
        {
            if(Level == 1)
                RainCloudSpawner.Instance.Activate();

            RainCloud.Instance.LevelUp(Damage, FireSpeed);
        }

        public override void RunFrame(float deltaTime)
        {
            if (RainCloudSpawner.Instance.isActiveAndEnabled)
                RainCloudSpawner.Instance.RunFrame(deltaTime);

            if (RainCloud.Instance.isActiveAndEnabled)
                RainCloud.Instance.RunFrame(deltaTime);
        }
    }
}
