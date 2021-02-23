using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets.GameTasks
{
    /// <summary>
    /// Runs a given FiniteTimeGameTask using a modified time scale.
    /// </summary>
    /// <inheritdoc/>
    public abstract class TimeModifyingGameTask : FiniteTimeGameTask
    {
        protected FiniteTimeGameTask InnerTask { get; set; }

        /// <summary>
        /// The delta time that was represented on the last frame.
        /// </summary>
        private float LastElapsedTime { get; set; }

        public TimeModifyingGameTask(FiniteTimeGameTask innerTask) : base(innerTask.Target, innerTask.Duration)
        {
            InnerTask = innerTask;
        }

        protected sealed override void OnFiniteTaskFrameRun(float deltaTime)
        {
            var innerDt = CalculateInnerDeltaTime();
            InnerTask.RunFrame(innerDt);
        }

        /// <summary>
        /// Calculates the delta time to be used to update the inner task.
        /// </summary>
        /// <returns>The delta time of the inner task.</returns>
        private float CalculateInnerDeltaTime()
        {
            var thisRatioComplete = Timer.RatioComplete;
            var thisElapsedTime = ModifyCompletionRatio(thisRatioComplete) * Duration;

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
    }
}
