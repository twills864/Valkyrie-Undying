using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.UI
{
    [Serializable]
    public struct BulletTrailInfo
    {
        public bool UseTrail;
        public float TrailTime;
        public float TangentCurveScale;
        //public float StartWidth;
    }
}
