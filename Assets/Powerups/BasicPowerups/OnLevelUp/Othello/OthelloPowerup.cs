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
        public override int MaxLevel => 2;

        protected override void InitBalance(in PowerupBalanceManager.OnLevelUpBalance balance)
        {
            float damageBase = balance.Othello.Damage.Base;
            float damageIncrease = balance.Othello.Damage.Increase;
            DamageCalculator = new SumLevelValueCalculator(damageBase, damageIncrease);
        }

        public int Damage => (int)DamageCalculator.Value;
        private SumLevelValueCalculator DamageCalculator { get; set; }

        public override void OnLevelUp()
        {
            Othello.Instance.LevelUp(Level, Damage);
        }
    }
}
