﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets
{
    [Serializable]
    [Obsolete("Replaced with EnemyExpOverride")]
    public struct EnemyPowerupDropChanceOverride
    {
        public bool ShouldOverride;
        public float Multiplier;

        public float? Value => ShouldOverride ? (float?)Multiplier : null;
    }
}
