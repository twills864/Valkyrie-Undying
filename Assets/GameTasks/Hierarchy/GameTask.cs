using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets.GameTasks
{
    public abstract class GameTask
    {
        protected GameTaskRunner Target { get; }

        public virtual bool IsFinished => Timer.Activated;
        protected virtual FrameTimer Timer { get; set; }

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

        public GameTask(GameTaskRunner target, float duration)
        {
            Target = target;
            Duration = duration;
        }

        public abstract void RunFrame(float deltaTime);
    }
}
