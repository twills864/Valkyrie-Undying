using System.Diagnostics;

namespace Assets.Util
{
    public abstract class FrameTimerBase
    {
        public float Elapsed { get; protected set; }
        public float ActivationInterval { get; protected set; }
        public bool Activated { get; protected set; }
        public float OverflowDeltaTime { get; protected set; }

        public float TimeUntilActivation => ActivationInterval - Elapsed;
        public float RatioComplete => Elapsed / ActivationInterval;
        public float RatioRemaining => 1f - RatioComplete;

        public abstract void Increment(float deltaTime);

        public bool UpdateActivates(float deltaTime)
        {
            Increment(deltaTime);
            return Activated;
        }

        public bool UpdateActivates(float deltaTime, out float overflowDeltaTime)
        {
            Increment(deltaTime);
            overflowDeltaTime = OverflowDeltaTime;
            return Activated;
        }

        /// <summary>
        /// Resets this timer, setting Elapsed to 0, and Activated to false.
        /// </summary>
        public void Reset()
        {
            Elapsed = 0;
            Activated = false;
        }

        /// <summary>
        /// Forces this timer to activate itself by setting its elapsed time
        /// equal to its activation interval.
        /// </summary>
        public void ActivateSelf()
        {
            Elapsed = ActivationInterval;
            Activated = true;
        }

        /// <summary>
        /// Resets this timer and forces it to activate itself by setting Elapsed to 0, and Activated to false.
        /// </summary>
        public void ResetAndActivateSelf()
        {
            Activated = false;
            ActivateSelf();
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected string DebuggerDisplayBase
        {
            get
            {
                // Don't use .ToString("P0") to remove space before percent sign
                string percent = (RatioComplete * 100).ToString("0");
                string timeUntilActivation = TimeUntilActivation.ToString("0.##");
                string elapsed = Elapsed.ToString("0.##");
                string activationInterval = ActivationInterval.ToString("0.##");

                string ret = $"{{{percent}%}} [-{timeUntilActivation}] ({elapsed} -> {activationInterval})";
                return ret;
            }
        }
    }
}
