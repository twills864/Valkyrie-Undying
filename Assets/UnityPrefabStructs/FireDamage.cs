using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.UnityPrefabStructs
{
    /// <summary>
    /// Contains information about fire damage, including what value the
    /// damage should start at, how quickly it should increase, and
    /// what the maximum amount of damage per second should be.
    /// </summary>
    /// <inheritdoc/>
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
