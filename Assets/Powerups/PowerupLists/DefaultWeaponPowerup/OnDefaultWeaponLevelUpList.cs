using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Powerups
{
    public class OnDefaultWeaponLevelUpList : PowerupList<OnDefaultWeaponLevelUpPowerup>
    {
        public OnDefaultWeaponLevelUpList(int powerupManagerIndex) : base(powerupManagerIndex)
        {

        }
    }
}
