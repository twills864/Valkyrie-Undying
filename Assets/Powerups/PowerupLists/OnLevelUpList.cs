using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Powerups
{
    /// <inheritdoc/>
    public class OnLevelUpList : PowerupList<OnLevelUpPowerup>
    {
        public OnLevelUpList(int powerupManagerIndex) : base(powerupManagerIndex)
        {

        }
    }
}
