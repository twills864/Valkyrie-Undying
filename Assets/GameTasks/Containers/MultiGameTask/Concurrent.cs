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
    /// Runs a series of FiniteTimeGameTasks in parallel.
    /// </summary>
    /// <inheritdoc/>
    public class ConcurrentGameTask : MultiGameTask
    {
        public ConcurrentGameTask(GameTaskRunner target, params FiniteTimeGameTask[] innerTasks)
            : base(target, innerTasks.Max(x => x.Duration), innerTasks)
        {
        }

        protected sealed override void OnFiniteTaskFrameRun(float deltaTime)
        {
            foreach (var task in InnerTasks)
                task.RunFrame(deltaTime);
        }
    }
}