using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Powerups
{
    /// <summary>
    /// Represents a powerup that will trigger a specific functionality
    /// when the powerup itself is collected.
    /// For example, a powerup of this type may set a flag in the Game Manager instance
    /// to start making additional checks each frame.
    /// </summary>
    /// <inheritdoc/>
    public abstract class OnLevelUpPowerup : Powerup
    {
    }
}
