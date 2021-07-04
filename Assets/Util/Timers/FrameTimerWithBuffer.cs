using System.Diagnostics;
using UnityEngine;

namespace Assets.Util
{
    /// <summary>
    /// A timer that activates after reaching a specified threshold,
    /// and remains active until it's reset.
    ///
    /// Contains an overflow buffer that can "hold" a small amount of
    /// extra time, in case a timer is designed to be constantly updated,
    /// but not deactivated until it's handled manually.
    /// </summary>
    /// <inheritdoc/>
    public class FrameTimerWithBuffer : FrameTimerBase
    {
        public const float DefaultBuffer = 1 / 30f;

        public float MaxTime { get; }
        public float TotalTime { get; set; }
        public FrameTimerWithBuffer(float activationInterval, float buffer = DefaultBuffer)
        {
            ActivationInterval = activationInterval;
            MaxTime = activationInterval + buffer;
        }

        public override void Increment(float deltaTime)
        {
            TotalTime += deltaTime;
            TotalTime = Mathf.Min(TotalTime, MaxTime);

            if (TotalTime < ActivationInterval)
            {
                Activated = false;
                Elapsed = TotalTime;
                OverflowDeltaTime = 0f;
            }
            else
            {
                // OverflowDeltaTime isn't guaranteed to make sense in the context of a FrameTimerWithBuffer.
                OverflowDeltaTime = Elapsed - ActivationInterval;
                Elapsed = ActivationInterval;
                Activated = true;
            }
        }

        protected override void OnReset()
        {
            TotalTime = 0f;
        }

        public override bool Activated
        {
            get
            {
                bool ret = base.Activated;
                if (ret)
                {
                    TotalTime -= ActivationInterval;
                    TouchTimer();
                }
                return ret;
            }

            protected set => base.Activated = value;
        }

        public static FrameTimerWithBuffer Default()
        {
            var ret = new FrameTimerWithBuffer(1.0f);
            ret.ActivateSelf();
            return ret;
        }

        public override string ToString()
        {
            string active = base.Activated ? "Activated" : "Inactive";
            string ret = $"({active}) ({TotalTime} / {MaxTime}) {DebuggerDisplayBase}";
            return ret;
        }
    }
}
