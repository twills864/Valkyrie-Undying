using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Powerups
{
    /// <summary>
    /// Represents a powerup that will activate when the player takes damage.
    /// </summary>
    /// <inheritdoc/>
    public abstract class OnGetHitPowerup : Powerup
    {
        public override void OnLevelUp() { }
        public abstract void OnGetHit();
    }
}
