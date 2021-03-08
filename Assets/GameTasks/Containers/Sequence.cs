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
    /// Runs a sequence of FiniteTimeGameTasks one after another.
    /// </summary>
    /// <inheritdoc/>
    public class SequenceGameTask : FiniteTimeGameTask
    {
        protected int CurrentIndex;
        protected int LastIndex;
        protected FiniteTimeGameTask[] InnerTasks { get; set; }

        protected FiniteTimeGameTask CurrentTask => InnerTasks[CurrentIndex];
        private FiniteTimeGameTask LastTask { get; }
        protected bool OnLastTask => CurrentIndex == LastIndex;

        public SequenceGameTask(GameTaskRunner target, params FiniteTimeGameTask[] innerTasks)
            : base(target, innerTasks.Sum(x => x.Duration))
        {
            InnerTasks = innerTasks;
            LastIndex = InnerTasks.Length - 1;
        }

        protected sealed override void OnFiniteTaskFrameRun(float deltaTime)
        {
            CurrentTask.RunFrame(deltaTime);

            while(CurrentTask.IsFinished && !OnLastTask)
            {
                float overflowDt = CurrentTask.OverflowDeltaTime;
                CurrentIndex++;
                CurrentTask.RunFrame(overflowDt);
            }

            // A rare edge case could occur when the last tasks have a duration close to 0,
            // and loss of floating point precision causes the base timer to activate
            // before activating each last task.
            if (IsFinished)
            {
                while (!CurrentTask.IsFinished)
                    CurrentTask.RunRemainingTime();
            }
        }

        public override void ResetSelf()
        {
            base.ResetSelf();

            CurrentIndex = 0;

            foreach (var task in InnerTasks)
                task.ResetSelf();
        }
    }
}