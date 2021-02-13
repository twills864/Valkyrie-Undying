using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Powerups
{
    public class OnHitList : PowerupList<OnHitPowerup>
    {
        public OnHitList(int powerupManagerIndex) : base(powerupManagerIndex)
        {

        }
    }
}
