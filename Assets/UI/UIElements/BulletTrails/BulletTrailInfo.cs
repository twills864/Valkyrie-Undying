using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.UI
{
    /// <summary>
    /// Unity prefab struct containing information about a bullet's bullet trail.
    /// </summary>
    [Serializable]
    public struct BulletTrailInfo
    {
        public bool UseTrail;
        public float TrailTime;
        public float TangentCurveScale;
        //public float StartWidth;
    }
}
