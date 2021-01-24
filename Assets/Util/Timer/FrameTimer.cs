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
    public class FrameTimer : FrameTimerBase
    {
        public FrameTimer(float activationInterval)
        {
            ActivationInterval = activationInterval;
        }

        public override void Increment(float deltaTime)
        {
            if(!Activated)
            {
                Elapsed += deltaTime;

                if (Elapsed >= ActivationInterval)
                {
                    Elapsed = ActivationInterval;
                    Activated = true;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected string DebuggerDisplay
        {
            get
            {
                string active = Activated ? "Activated" : "Inactive";
                string ret = $"({active}) {DebuggerDisplayBase}";
                return ret;
            }
        }
    }
}
