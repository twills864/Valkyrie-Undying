using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets
{
    [Serializable]
    public struct EnemyExpOverride
    {
        public bool ShouldOverride;
        public float Multiplier;

        public float? Value => ShouldOverride ? (float?)Multiplier : null;
    }
}
