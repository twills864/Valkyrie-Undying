using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;
using LogUtilAssets;

namespace Assets.GameTasks
{
    public abstract class GameTask : Loggable
    {
        public override string LogTagColor => "#FFFF00";

        public GameTaskRunner Target { get; }

        public virtual bool IsFinished => Timer.Activated;
        protected virtual FrameTimerBase Timer { get; set; }


        public GameTask(GameTaskRunner target)
        {
            Target = target;
        }

        public abstract void RunFrame(float deltaTime);
    }
}
