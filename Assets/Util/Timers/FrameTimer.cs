using System.Diagnostics;

namespace Assets.Util
{
    /// <inheritdoc/>
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
                    OverflowDeltaTime = Elapsed - ActivationInterval;
                    Elapsed = ActivationInterval;
                    Activated = true;
                }
            }
        }

        public static FrameTimer Default()
        {
            var ret = new FrameTimer(1.0f);
            ret.ActivateSelf();
            return ret;
        }

        public override string ToString()
        {
            string active = Activated ? "Activated" : "Inactive";
            string ret = $"({active}) {DebuggerDisplayBase}";
            return ret;
        }
    }
}
