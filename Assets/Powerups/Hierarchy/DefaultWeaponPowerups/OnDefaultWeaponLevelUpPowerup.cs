using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Powerups.Balance;

namespace Assets.Powerups
{
    /// <summary>
    /// Represents a powerup that will trigger a specific functionality
    /// when the powerup itself is collected.
    /// Powerups are not programmatically required to be related to default bullets,
    /// but should be in order to allow the game to properly select which type of
    /// powerup to give the player.
    /// </summary>
    /// <inheritdoc/>
    public abstract class OnDefaultWeaponLevelUpPowerup : DefaultWeaponPowerup
    {
        protected sealed override void InitBalance(in PowerupBalanceManager balance)
            => InitBalance(in balance.OnDefaultWeaponLevelUp);

        protected abstract void InitBalance(in PowerupBalanceManager.OnDefaultWeaponLevelUpBalance balance);
    }
}