﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Powerups.Balance;

namespace Assets.Powerups
{
    public class OnGetHitList : PowerupList<OnGetHitPowerup>
    {
        public OnGetHitList(int powerupManagerIndex) : base(powerupManagerIndex)
        {

        }
    }
}
