using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets
{
    /// <summary>
    /// Allows information about how long a player should remain invincible after taking damage
    /// to be fine-tuned from the Unity editor.
    /// </summary>
    /// <inheritdoc/>
    [Serializable]
    public struct PlayerIFrames
    {
        public float BlinkFadeOutAlpha;
        public float BlinkFadeInAlpha;
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
