using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets.GameTasks
{
    /// <summary>
    /// Represents a GameTask that will run for an unlimited amount of time,
    /// and will never be considered to have run to completion.
    /// </summary>
    /// <inheritdoc/>
    public abstract class InfiniteTimeGameTask : GameTask
    {
        public InfiniteTimeGameTask(ValkyrieSprite target) : base(target)
        {
            Timer = new InactiveLoopingFrameTimer();
        }

        public override bool IsFinished => false;
    }
}
