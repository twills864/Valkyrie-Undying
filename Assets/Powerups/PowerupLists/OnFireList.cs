using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Powerups.Balance;

namespace Assets.Powerups
{
    /// <inheritdoc/>
    public class OnFireList : PowerupList<OnFirePowerup>
    {
        public OnFireList(int powerupManagerIndex) : base(powerupManagerIndex)
        {

        }
    }
}
