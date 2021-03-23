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
    /// Spawns a passive rain cloud behind the player that rains bullets on enemies.
    /// </summary>
    /// <inheritdoc/>
    public class MonsoonPowerup : OnLevelUpPowerup
    {
        protected override void InitBalance(in PowerupBalanceManager.OnLevelUpBalance balance)
        {
            float fireSpeedBase = balance.Monsoon.FireSpeed.Base;
            float fireSpeedIncrease = balance.Monsoon.FireSpeed.Increase;
            FireSpeedCalculator = new ProductLevelValueCalculator(fireSpeedBase, fireSpeedIncrease);

            float damageBase = balance.Monsoon.Damage.Base;
            float damageIncrease = balance.Monsoon.Damage.IncreasePerLevel;
            DamageCalculator = new SumLevelValueCalculator(damageBase, damageIncrease);
        }

        public float FireSpeed => FireSpeedCalculator.Value;
        private ProductLevelValueCalculator FireSpeedCalculator { get; set; }

        public int Damage => (int) DamageCalculator.Value;
        private SumLevelValueCalculator DamageCalculator { get; set; }

        public override void OnLevelUp()
        {
            if(Level == 1)
                MonsoonSpawner.Instance.Activate();

            Monsoon.Instance.LevelUp(Damage, FireSpeed);
        }
    }
}
