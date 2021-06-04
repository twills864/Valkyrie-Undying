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
    /// that directly and exclusively upgrades the player's default weapon.
    /// </summary>
    /// <inheritdoc/>
    public abstract class DefaultWeaponPowerup : Powerup
    {
        public sealed override bool IsDefaultWeaponPowerup => true;
    }
}
