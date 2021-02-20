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
    public class RainCloudPowerup : OnLevelUpPowerup
    {
        private const float FireSpeedBase = 0.5f;
        private const float FireSpeedIncrease = 0.9f;

        protected override LevelValueCalculator InitialValueCalculator
            => new ProductLevelValueCalculator(FireSpeedBase, FireSpeedIncrease);

        public float FireSpeed => ValueCalculator.Value;

        private const float DamageBase = 10;
        private const float DamageIncrease = 1;
        private SumLevelValueCalculator DamageCalculator =
            new SumLevelValueCalculator(DamageBase, DamageIncrease);

        public int Damage => (int) DamageCalculator.Value;

        public override void OnLevelUp()
        {
            if(Level == 1)
                RainCloudSpawner.Instance.Activate();

            DamageCalculator.Level = Level;
            RainCloud.Instance.LevelUp(Damage, FireSpeed);
        }
    }
}
