﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Powerups
{
    public class OnFireList : PowerupList<OnFirePowerup>
    {
        public OnFireList(int powerupManagerIndex) : base(powerupManagerIndex)
        {

        }
    }
}