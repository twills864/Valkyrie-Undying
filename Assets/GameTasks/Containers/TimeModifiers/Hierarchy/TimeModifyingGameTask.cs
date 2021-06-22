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
        protected TimeModifyingFrameTimer TimeModifyingTimer { get; private set; }

        protected override void OnDurationSet(float value)
        {
            if (InnerTask != null)
                InnerTask.Duration = value;
        }

        public TimeModifyingGameTask(FiniteTimeGameTask innerTask) : base(innerTask.Target, innerTask.Duration)
        {
            InnerTask = innerTask;
        }

        protected sealed override void OnFiniteTaskFrameRun(float deltaTime)
        {
            var innerDt = TimeModifyingTimer.RepresentedDeltaTime;
            InnerTask.RunFrame(innerDt);
        }

        protected abstract TimeModifyingFrameTimer DefaultTimeModifyingFrameTimer(float duration);
        protected sealed override FrameTimer DefaultFrameTimer(float duration)
        {
            TimeModifyingTimer = DefaultTimeModifyingFrameTimer(duration);
            return TimeModifyingTimer;
        }

        public override void ResetSelf()
        {
            base.ResetSelf();

            InnerTask.ResetSelf();
        }
    }
}
