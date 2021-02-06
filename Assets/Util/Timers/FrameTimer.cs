using System.Diagnostics;

namespace Assets.Util
{
    /// <inheritdoc/>
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
        protected virtual string DebuggerDisplay
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
