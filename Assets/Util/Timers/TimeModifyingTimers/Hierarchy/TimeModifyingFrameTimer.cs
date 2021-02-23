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
                var innerDt = CalculateInnerDeltaTime();
                base.Increment(innerDt);
            }
        }

        /// <summary>
        /// The actual amount of time that this timer has counted.
        /// </summary>
        private float TrueElapsedTime { get; set; }

        /// <summary>
        /// The actual completion ratio that this timer has counted.
        /// </summary>
        private float TrueElapsedRatio => TrueElapsedTime / ActivationInterval;

        /// <summary>
        /// The delta time that was represented on the last frame.
        /// </summary>
        private float LastElapsedTime { get; set; }

        /// <summary>
        /// Calculates the delta time to be used to update the inner task.
        /// </summary>
        /// <returns>The delta time of the inner task.</returns>
        private float CalculateInnerDeltaTime()
        {
            var thisElapsedTime = CalculateCurrentElapsedTime();
            var innerDt = thisElapsedTime - LastElapsedTime;
            LastElapsedTime = thisElapsedTime;
            return innerDt;
        }

        /// <summary>
        /// The function representing the time scale of the inner task.
        /// Should be a continuous, increasing function that begins at (0, 0)
        /// and ends at (1, 1).
        /// </summary>
        /// <param name="currentRatioComplete">The current completion ratio of
        /// the timer representing this task.</param>
        /// <returns>The modified completion ratio to be used to represent
        /// the delta time of the inner task.</returns>
        /// <example>The EaseOut task uses the function y = x^2 as its time scale.
        /// Therefore, its method is structured as follows:
        /// <code>
        /// protected override float ModifyCompletionRatio(float currentRatioComplete)
        /// {
        ///     return currentRatioComplete * currentRatioComplete;
        /// }</code></example>
        protected abstract float ModifyCompletionRatio(float currentRatioComplete);

        protected override void OnReset()
        {
            TrueElapsedTime = 0f;
            LastElapsedTime = 0f;
        }

        protected abstract float InverseRatio(float currentRatioComplete);
        public sealed override float RatioComplete
        {
            get => base.RatioComplete;
            set
            {
                base.RatioComplete = value;
                TrueElapsedTime = InverseRatio(value) * ActivationInterval;
                LastElapsedTime = CalculateCurrentElapsedTime();

            }
        }

        private float CalculateCurrentElapsedTime()
        {
            var ret = ModifyCompletionRatio(TrueElapsedRatio) * ActivationInterval;
            return ret;
        }
    }
}
