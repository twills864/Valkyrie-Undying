using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Powerups.Balance;

namespace Assets.Powerups
{
    /// <summary>
    /// Represents a powerup that will run frames when active.
    /// </summary>
    /// <inheritdoc/>
    public abstract class PassivePowerup : Powerup
    {
        public abstract void RunFrame(float deltaTime);
        public override void OnLevelUp() { }

        protected sealed override void InitBalance(in PowerupBalanceManager balance)
            => InitBalance(in balance.Passive);

        protected abstract void InitBalance(in PowerupBalanceManager.PassiveBalance balance);
    }
}
