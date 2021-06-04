using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Powerups.Balance;

namespace Assets.Powerups
{
    /// <summary>
    /// Represents a powerup that will activate when the player takes damage.
    /// </summary>
    /// <inheritdoc/>
    public abstract class OnGetHitPowerup : BasicPowerup
    {
        public override void OnLevelUp() { }
        public abstract void OnGetHit();

        protected sealed override void InitBalance(in PowerupBalanceManager balance)
            => InitBalance(in balance.OnGetHit);

        protected abstract void InitBalance(in PowerupBalanceManager.OnGetHitBalance balance);
    }
}
