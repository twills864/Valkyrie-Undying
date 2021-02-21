using System.Diagnostics;

namespace Assets.Util
{
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
