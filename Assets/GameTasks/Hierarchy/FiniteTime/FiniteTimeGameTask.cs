using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets.GameTasks
{
    public abstract class FiniteTimeGameTask : GameTask
    {
        private float _duration;
        public virtual float Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                Timer = new FrameTimer(value);
            }
        }

        public FiniteTimeGameTask(GameTaskRunner target, float duration) : base(target)
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
    }
}
