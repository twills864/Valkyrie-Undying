using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util
{
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class LoopingFrameTimer : FrameTimerBase
    {
        public LoopingFrameTimer(float activationInterval)
        {
            ActivationInterval = activationInterval;
        }

        public override void Increment(float deltaTime)
        {
            Elapsed += deltaTime;

            if (Elapsed < ActivationInterval)
            {
                Activated = false;
                //overflowTime = 0f;
            }
            else
            {
                Activated = true;
                Elapsed -= ActivationInterval;
                //overflowTime = Elapsed;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay
        {
            get
            {
                string active = Activated ? "Activated this frame" : "Inactive";
                string ret = $"({active}) {DebuggerDisplayBase}";
                return ret;
            }
        }
    }
}
