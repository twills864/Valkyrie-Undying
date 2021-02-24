using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogUtilAssets;

namespace Assets.Util
{

    /// <summary>
    /// Runs a given FrameTimer using a modified time scale.
    /// </summary>
    /// <inheritdoc/>
    public abstract class TimeModifyingFrameTimer : FrameTimer
    {
        /// <summary>
        /// The actual amount of time that this timer has counted.
        /// </summary>
        private float TrueElapsedTime { get; set; }

        /// <summary>
        /// The actual completion ratio that this timer has counted.
        /// </summary>
        private float TrueElapsedRatio => TrueElapsedTime / ActivationInterval;

        /// <summary>
        /// The elapsed time that was represented on the last frame.
        /// </summary>
        private float LastRepresentedElapsedTime { get; set; }

        /// <summary>
        /// The deltaTime that was represented on this frame.
        /// </summary>
        public float RepresentedDeltaTime { get; set; }


        public TimeModifyingFrameTimer(float activationInterval) : base(activationInterval)
        {

        }

        public sealed override void Increment(float deltaTime)
        {
            if (!Activated)
            {
                TrueElapsedTime += deltaTime;
                if (TrueElapsedTime > ActivationInterval)
                    TrueElapsedTime = ActivationInterval;
                CalculateRepresentedDeltaTime();

                base.IncrementConfirmed(RepresentedDeltaTime);
            }
        }

        /// <summary>
        /// Calculates the delta time to be used to update the inner timer,
        /// saves the result to RepresentedDeltaTime.
        /// LastRepresentedElapsedTime is also updated in the process.
        /// </summary>
        private void CalculateRepresentedDeltaTime()
        {
            var thisElapsedTime = CalculateCurrentElapsedTime();
            RepresentedDeltaTime = thisElapsedTime - LastRepresentedElapsedTime;
            LastRepresentedElapsedTime = thisElapsedTime;
        }


        /// <summary>
        /// The inverse of the function represented by Adjust().
        /// </summary>
        /// <param name="currentRatioComplete">The current completion ratio of this timer.</param>
        /// <returns>The inverse of the adjusted completion ratio to be used to represent
        /// the elapsed ratio of this timer.</returns>
        /// <example>The EaseOutTimer uses the function y = x^2 as its time scale.
        /// The inverse of this function is x = y^2, which is equivalent to y = sqrt(x).
        /// Therefore, its method is structured as follows:
        /// <code>
        /// protected override float Invert(float currentRatioComplete)
        /// {
        ///     return Mathf.Sqrt(currentRatioComplete);
        /// }</code></example>
        protected abstract float Invert(float currentRatioComplete);
        public sealed override float RatioComplete
        {
            get => base.RatioComplete;
            set
            {
                base.RatioComplete = value;
                TrueElapsedTime = Invert(value) * ActivationInterval;
                LastRepresentedElapsedTime = CalculateCurrentElapsedTime();

            }
        }

        /// <summary>
        /// The function representing the time scale of this timer.
        /// Should be a continuous, increasing function that begins at (0, 0)
        /// and ends at (1, 1).
        /// </summary>
        /// <param name="currentRatioComplete">The current completion ratio of this timer.</param>
        /// <returns>The adjusted completion ratio to be used to represent
        /// the elapsed ratio of this timer.</returns>
        /// <example>The EaseOutTimer uses the function y = x^2 as its time scale.
        /// Therefore, its method is structured as follows:
        /// <code>
        /// protected override float Adjust(float currentRatioComplete)
        /// {
        ///     return currentRatioComplete * currentRatioComplete;
        /// }</code></example>
        protected abstract float Adjust(float currentRatioComplete);
        private float CalculateCurrentElapsedTime()
        {
            var ret = Adjust(TrueElapsedRatio) * ActivationInterval;
            return ret;
        }

        /// <summary>
        /// Also clears subclass-related timer properties.
        /// </summary>
        protected override void OnReset()
        {
            TrueElapsedTime = 0f;
            LastRepresentedElapsedTime = 0f;
        }
    }
}
