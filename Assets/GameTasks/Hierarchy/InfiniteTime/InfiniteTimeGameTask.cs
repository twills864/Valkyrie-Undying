using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets.GameTasks
{
    public abstract class InfiniteTimeGameTask : GameTask
    {
        public InfiniteTimeGameTask(ValkyrieSprite target) : base(target)
        {
            Timer = new InactiveLoopingFrameTimer();
        }

        public override bool IsFinished => false;
    }
}
