using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets
{
    [Serializable]
    public struct PlayerIFrames
    {
        public float BlinkFadeAlpha;
        public BlinkInfo StandardBlinks;
        public BlinkInfo FinalBlinks;
        public float GracePeriodTime;

        [Serializable]
        public struct BlinkInfo
        {
            public int NumBlinks;
            public float OneBlinkTime;
        }
    }
}
