﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Powerups
{
    /// <inheritdoc/>
    public class OnDefaultWeaponKillList : PowerupList<OnDefaultWeaponKillPowerup>
    {
        public OnDefaultWeaponKillList(int powerupManagerIndex) : base(powerupManagerIndex)
        {

        }
    }
}
