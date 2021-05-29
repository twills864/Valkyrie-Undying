using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets.GameTasks
{
    /// <inheritdoc/>
    public abstract class FiniteTimeGameTask : GameTask
    {
        private float _duration;

        /// <summary>
        /// The duration that this Task will run for.
        /// </summary>
        public virtual float Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                Timer = DefaultFrameTimer(value);
            }
        }

        protected virtual FrameTimer DefaultFrameTimer(float duration)
        {
            var ret = new FrameTimer(duration);
            return ret;
        }

        public float RatioComplete => Timer.RatioComplete;
        public float RatioRemaining => Timer.RatioRemaining;
        public float OverflowDeltaTime => Timer.OverflowDeltaTime;

        public FiniteTimeGameTask(ValkyrieSprite target, float duration) : base(target)
        {
            Duration = duration;
        }

        protected abstract void OnFiniteTaskFrameRun(float deltaTime);
        public sealed override void RunFrame(float deltaTime)
        {
            if (!Timer.Activated)
            {
                Timer.Increment(deltaTime);
                OnFiniteTaskFrameRun(deltaTime);
            }
        }

        public void RunRemainingTime()
        {
            var deltaTime = Timer.TimeUntilActivation;
            RunFrame(deltaTime);
            Timer.ActivateSelf();
        }

        /// <summary>
        /// Forces the completion of this task by activating its timer.
        /// </summary>
        public virtual void FinishSelf()
        {
            Timer.ActivateSelf();
        }

        protected virtual string ToFiniteTimeGameTaskString() => "";
        protected sealed override string ToGameTaskString()
        {
            return $"{Duration} {ToFiniteTimeGameTaskString()}";
        }
    }
}
