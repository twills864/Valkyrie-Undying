using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets
{
    [Serializable]
    public struct DirectorBalance
    {
        public float BaseEnemyPowerupDropChance;

        public float WeaponLevelOverrideChanceFlatAddition;

        public float OneUpOverrideChance;



        public int InitialTargetEnemiesOnScreen;
    }
}
