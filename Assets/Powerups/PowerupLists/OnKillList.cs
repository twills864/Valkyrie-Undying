using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Powerups
{
    /// <inheritdoc/>
    public class OnKillList : PowerupList<OnKillPowerup>
    {
        public OnKillList(int powerupManagerIndex) : base(powerupManagerIndex)
        {

        }
    }
}
