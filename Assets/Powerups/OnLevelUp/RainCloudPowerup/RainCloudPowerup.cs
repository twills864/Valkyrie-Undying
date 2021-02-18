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
        private const float FireSpeedBase = 0.6f;
        private const float FireSpeedIncrease = 0.9f;

        protected override LevelValueCalculator InitialValueCalculator
            => new ProductLevelValueCalculator(FireSpeedBase, FireSpeedIncrease);

        public float Firespeed => ValueCalculator.Value;


        private const float DamageBase = 10;
        private const float DamageIncrease = 1;
        private SumLevelValueCalculator DamageCalculator =
            new SumLevelValueCalculator(DamageBase, DamageIncrease);

        public int Damage => (int) DamageCalculator.Value;

        public override void OnLevelUp()
        {
            DamageCalculator.Level = Level;
            //GameManager.Instance.PlayerFireDeltaTimeScale = PlayerFireDeltaTimeScale;
        }
    }
}
