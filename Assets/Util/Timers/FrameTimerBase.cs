using System.Diagnostics;

namespace Assets.Util
{
    public abstract class FrameTimerBase
    {
        private float _activationInterval;

        public float Elapsed { get; set; }


        public float ActivationInterval
        {
            get => _activationInterval;
            set
            {
                _activationInterval = value;
                TouchTimer();
            }
        }
        public virtual bool Activated { get; protected set; }
        public float OverflowDeltaTime { get; protected set; }

        public float TimeUntilActivation
        {
            get => ActivationInterval - Elapsed;
            set => Elapsed = ActivationInterval - value;
        }

        public virtual float RatioComplete
        {
            get => Elapsed / ActivationInterval;
            set
            {
                Elapsed = value * ActivationInterval;
                TouchTimer();
            }
        }
        public virtual float RatioRemaining
        {
            get => 1f - RatioComplete;
            set
            {
                Elapsed = (1 - value) * ActivationInterval;
                TouchTimer();
            }
        }

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

        protected virtual void OnReset() { }
        /// <summary>
        /// Resets this timer, setting Elapsed to 0, and Activated to false.
        /// </summary>
        public void Reset()
        {
            Elapsed = 0;
            Activated = false;
            OnReset();
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
        /// "Increments" the timer by 0 in order to recalculate the value of Activated.
        /// Named after the bash command "touch."
        /// </summary>
        public void TouchTimer()
        {
            Activated = false;
            Increment(0f);
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
