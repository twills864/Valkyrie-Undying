using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;
using LogUtilAssets;

namespace Assets.GameTasks
{
    /// <inheritdoc/>
    public abstract class GameTask : Loggable
    {
        public override string LogTagColor => "#FFFF00";

        /// <summary>
        /// The GameTaskRunner that will receive the effects of this GameTask.
        /// </summary>
        public GameTaskRunner Target { get; }

        /// <summary>
        /// Whether or not this GameTask has finished running.
        /// </summary>
        public virtual bool IsFinished => Timer.Activated;

        /// <summary>
        /// The FrameTimer that will time the events of this GameTask.
        /// </summary>
        protected virtual FrameTimerBase Timer { get; set; }


        public GameTask(GameTaskRunner target)
        {
            Target = target;
        }

        public abstract void RunFrame(float deltaTime);
    }
}
