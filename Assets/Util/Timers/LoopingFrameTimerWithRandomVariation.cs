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
    public class LoopingFrameTimerWithRandomVariation : LoopingFrameTimer
    {
        protected float Variance { get; }

        public LoopingFrameTimerWithRandomVariation(float activationInterval, float plusOrMinusVariance)
            :base(activationInterval - plusOrMinusVariance)
        {
            Variance = plusOrMinusVariance * 2;
        }

        public override void Increment(float deltaTime)
        {
            Elapsed += deltaTime;

            if (Elapsed < ActivationInterval)
            {
                Activated = false;
            }
            else
            {
                Activated = true;
                Elapsed = Elapsed - ActivationInterval - RandomUtil.Float(Variance);
            }
        }

        protected override string DebuggerDisplay
        {
            get
            {
                float activationInterval = ActivationInterval + (Variance * 0.5f);
                var ret = $"[{activationInterval.ToString("0.##")} ±{Variance.ToString("0.##")}] {base.DebuggerDisplay}";
                return ret;
            }
        }
    }
}
