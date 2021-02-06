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
        protected virtual string DebuggerDisplay
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
