using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.Util;

namespace Assets.Powerups
{
    /// <summary>
    /// Represents a powerup that can be collected by the player
    /// that doesn't directly and exclusively upgrade the player's default weapon.
    /// </summary>
    /// <inheritdoc/>
    public abstract class BasicPowerup : Powerup
    {
        public sealed override bool IsDefaultWeaponPowerup => false;
    }
}
