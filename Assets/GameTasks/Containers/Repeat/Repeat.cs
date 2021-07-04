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
    /// Repeats the given FiniteTimeGameTask a finite number of times.
    /// </summary>
    /// <inheritdoc/>
    public class Repeat : FiniteTimeGameTask
    {
        protected FiniteTimeGameTask InnerTask { get; set; }

        private int RepetitionsToComplete { get; }
        private int RepetitionsCompleted { get; set; }

        private bool HasRepetitionsRemaining => RepetitionsCompleted < RepetitionsToComplete;

        public Repeat(FiniteTimeGameTask innerTask, int numberOfRepetitions) : base(innerTask.Target, innerTask.Duration * numberOfRepetitions)
        {
            InnerTask = innerTask;

            RepetitionsToComplete = numberOfRepetitions;
            RepetitionsCompleted = 0;
        }

        protected override void OnFiniteTaskFrameRun(float deltaTime)
        {
            while(HasRepetitionsRemaining && InnerTask.FrameRunFinishes(deltaTime))
            {
                RepetitionsCompleted++;

                deltaTime = InnerTask.OverflowDeltaTime;
                InnerTask.ResetSelf();
            }

            // A rare edge case could occur when the last task has a duration remaining close to 0,
            // and loss of floating point precision causes the base timer to activate
            // before activating the last task.
            if (IsFinished)
                InnerTask.RunRemainingTime();
        }

        public override void ResetSelf()
        {
            base.ResetSelf();

            InnerTask.ResetSelf();
            RepetitionsCompleted = 0;
        }
    }
}
