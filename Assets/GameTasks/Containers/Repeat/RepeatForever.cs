using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;
using UnityEngine;

namespace Assets.GameTasks
{
    /// <summary>
    /// Repeats the given FiniteTimeGameTask an unlimited number of times.
    /// </summary>
    public class RepeatForever : InfiniteTimeGameTask
    {
        protected FiniteTimeGameTask InnerTask { get; set; }

        public RepeatForever(FiniteTimeGameTask innerTask) : base(innerTask.Target)
        {
            InnerTask = innerTask;
        }

        public RepeatForever(params FiniteTimeGameTask[] innerTasks) : base(innerTasks[0].Target)
        {
            InnerTask = new Sequence(innerTasks);
        }

        public override void RunFrame(float deltaTime)
        {
            while(InnerTask.FrameRunFinishes(deltaTime))
            {
                deltaTime = InnerTask.OverflowDeltaTime;
                InnerTask.ResetSelf();
            }
        }

        public override void ResetSelf()
        {
            InnerTask.ResetSelf();
        }
    }
}
