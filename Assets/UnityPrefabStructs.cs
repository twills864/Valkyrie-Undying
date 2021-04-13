using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets
{
    [Serializable]
    public struct EnemyPowerupDropChanceOverride
    {
        public bool ShouldOverride;
        public float Multiplier;

        public float? Value => ShouldOverride ? (float?)Multiplier : null;
    }

    [Serializable]
    public struct DirectorBalance
    {
        public float BaseEnemyPowerupDropChance;

        public float WeaponLevelOverrideChance;

        public float OneUpOverrideChance;
    }
}
