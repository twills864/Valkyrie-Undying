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
    public class MonsoonPowerup : PassivePowerup
    {
        protected override void InitBalance(in PowerupBalanceManager.PassiveBalance balance)
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

        public override void RunFrame(float deltaTime)
        {
            if (MonsoonSpawner.Instance.isActiveAndEnabled)
                MonsoonSpawner.Instance.RunFrame(deltaTime);

            if (Monsoon.Instance.isActiveAndEnabled)
                Monsoon.Instance.RunFrame(deltaTime);
        }
    }
}
