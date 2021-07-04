using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Powerups
{
    /// <inheritdoc/>
    public class PassivePowerupList : PowerupList<PassivePowerup>
    {
        public PassivePowerupList(int powerupManagerIndex) : base(powerupManagerIndex)
        {

        }
    }
}
