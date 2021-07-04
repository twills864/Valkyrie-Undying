using System.Diagnostics;

namespace Assets.Util
{
    /// <summary>
    /// A timer that activates for one frame after reaching a specified threshold,
    /// before automatically resetting itself and starting the timing again.
    /// </summary>
    /// <inheritdoc/>
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
                OverflowDeltaTime = 0f;
            }
            else
            {
                Activated = true;
                Elapsed -= ActivationInterval;
                OverflowDeltaTime = Elapsed;
            }
        }

        public virtual LoopingFrameTimer CreateClone() => new LoopingFrameTimer(ActivationInterval);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected virtual string DebuggerDisplay
        {
            get
            {
                string active = Activated ? "Activated this frame" : "Inactive";
                string ret = $"({active}) {DebuggerDisplayBase}";
                return ret;
            }
        }

        public static LoopingFrameTimer Default()
        {
            return new LoopingFrameTimer(1.0f);
        }
    }
}
