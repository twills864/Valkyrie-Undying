using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.UnityPrefabStructs
{
    /// <summary>
    /// Contains a information about how often an enemy's weapon should be fired, and
    /// how much random variance there can be in the positive or negative direction.
    /// </summary>
    /// <inheritdoc/>
    [Serializable]
    public struct VariantFireSpeed
    {
        public float FireSpeed;
        public float FireSpeedVariance;

        public VariantFireSpeed(float fireSpeed, float fireSpeedVariance)
        {
            FireSpeed = fireSpeed;
            FireSpeedVariance = fireSpeedVariance;
        }
    }
}
