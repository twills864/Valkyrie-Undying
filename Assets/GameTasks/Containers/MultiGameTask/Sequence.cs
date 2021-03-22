﻿using System;
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
    public class Sequence : MultiGameTask
    {
        protected int CurrentIndex;
        protected int LastIndex;

        protected FiniteTimeGameTask CurrentTask => InnerTasks[CurrentIndex];
        private FiniteTimeGameTask LastTask { get; }
        protected bool OnLastTask => CurrentIndex == LastIndex;

        public Sequence(params FiniteTimeGameTask[] innerTasks)
            : base(innerTasks[0].Target, innerTasks.Sum(x => x.Duration), innerTasks)
        {
            LastIndex = InnerTasks.Length - 1;
        }

        protected sealed override void OnFiniteTaskFrameRun(float deltaTime)
        {
            CurrentTask.RunFrame(deltaTime);

            while(CurrentTask.IsFinished && !OnLastTask)
            {
                float overflowDt = CurrentTask.OverflowDeltaTime;
                CurrentIndex++;
                CurrentTask.ResetSelf();
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

        protected override void OnMultiGameTaskReset()
        {
            CurrentIndex = 0;
            CurrentTask.ResetSelf();
        }


        /// <summary>
        /// The default Sequence game task constructor.
        /// Should only be used for a Sequence that cannot be null, but will
        /// quickly be replaced by a meaningful Sequence.
        /// </summary>
        private Sequence() : base(null, 1.0f)
        {
        }
        public static Sequence Default() => new Sequence(Delay.Default());
    }
}