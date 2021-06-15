using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.UnityPrefabStructs
{
    [Serializable]
    public struct FireDamage
    {
        public int CollisionDamage;
        public int DamageIncreasePerTick;
        public int MaxDamage;

        public void Reset()
        {
            CollisionDamage = 0;
            DamageIncreasePerTick = 0;
            MaxDamage = 0;
        }
    }
}
