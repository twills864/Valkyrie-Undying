using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Powerups.Balance;
using Assets.UnityPrefabStructs;
using Assets.Util;

namespace Assets.Powerups
{
    /// <summary>
    /// Spawns a passive rain cloud behind the player that rains bullets on enemies.
    /// </summary>
    /// <inheritdoc/>
    public class MonsoonPowerup : OnLevelUpPowerup
    {
        protected override void InitBalance(in PowerupBalanceManager.OnLevelUpBalance balance)
        {
            float fireSpeedIncrease = balance.Monsoon.VariantFireSpeed.Increase;

            float fireSpeedBase = balance.Monsoon.VariantFireSpeed.FireSpeed;
            FireSpeedCalculator = new ProductLevelValueCalculator(fireSpeedBase, fireSpeedIncrease);

            float varianceBase = balance.Monsoon.VariantFireSpeed.Variance;
            VarianceCalculator = new ProductLevelValueCalculator(varianceBase, fireSpeedIncrease);

            float damageBase = balance.Monsoon.Damage.Base;
            float damageIncrease = balance.Monsoon.Damage.IncreasePerLevel;
            DamageCalculator = new SumLevelValueCalculator(damageBase, damageIncrease);
        }

        private VariantFireSpeed FireSpeed => new VariantFireSpeed(FireSpeedCalculator.Value, VarianceCalculator.Value);

        private ProductLevelValueCalculator FireSpeedCalculator { get; set; }
        private ProductLevelValueCalculator VarianceCalculator { get; set; }

        public int Damage => (int) DamageCalculator.Value;
        private SumLevelValueCalculator DamageCalculator { get; set; }

        public override void OnLevelUp()
        {
            Monsoon.Instance.LevelUp(Level, Damage, FireSpeed);
        }
    }
}
