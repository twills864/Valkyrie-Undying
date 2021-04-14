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
    public class OthelloPowerup : OnLevelUpPowerup
    {
        protected override void InitBalance(in PowerupBalanceManager.OnLevelUpBalance balance)
        {
            //float fireSpeedBase = balance.Othello.FireSpeed.Base;
            //float fireSpeedIncrease = balance.Othello.FireSpeed.Max;
            //FireSpeedModifierCalculator = new SumLevelValueCalculator(fireSpeedBase, fireSpeedIncrease);

            float damageBase = balance.Othello.Damage.Base;
            float damageIncrease = balance.Othello.Damage.Increase;
            DamageCalculator = new SumLevelValueCalculator(damageBase, damageIncrease);
        }

        //public float FireSpeedModifier => FireSpeedModifierCalculator.Value;
        //private SumLevelValueCalculator FireSpeedModifierCalculator { get; set; }

        public int Damage => (int)DamageCalculator.Value;
        private SumLevelValueCalculator DamageCalculator { get; set; }

        public override void OnLevelUp()
        {
            Othello.Instance.LevelUp(Level, Damage);
        }
    }
}
