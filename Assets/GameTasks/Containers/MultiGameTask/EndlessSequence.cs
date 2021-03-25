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
    /// Runs a sequence of FiniteTimeGameTasks one after another,
    /// with the final task being an InfiniteTimeGameTask.
    /// </summary>
    /// <inheritdoc/>
    public class EndlessSequence : InfiniteTimeGameTask
    {
        private Sequence InnerSequence { get; }
        private InfiniteTimeGameTask FinalTask { get; }

        private FrameBehaviorArray FrameBehaviors;

        public EndlessSequence(InfiniteTimeGameTask finalTask)
            : this(finalTask, Sequence.Default())
        {
        }
        public EndlessSequence(InfiniteTimeGameTask finalTask, params FiniteTimeGameTask[] innerTasks)
            : this(finalTask, new Sequence(innerTasks))
        {
        }
        public EndlessSequence(InfiniteTimeGameTask finalTask, Sequence sequence)
            : base(finalTask.Target)
        {
            InnerSequence = sequence;
            FinalTask = finalTask;

            FrameBehaviors = new FrameBehaviorArray(RunSequenceFrame, RunEndlessFrame);
        }

        public override void RunFrame(float deltaTime)
        {
            FrameBehaviors.CurrentBehavior(deltaTime);
        }

        private void RunSequenceFrame(float deltaTime)
        {
            if(InnerSequence.FrameRunFinishes(deltaTime))
            {
                FrameBehaviors.BehaviorIndex++;
                RunEndlessFrame(InnerSequence.OverflowDeltaTime);
            }
        }

        private void RunEndlessFrame(float deltaTime)
        {
            FinalTask.RunFrame(deltaTime);
        }

        public override void ResetSelf()
        {
            base.ResetSelf();
            FrameBehaviors.BehaviorIndex = 0;
            InnerSequence.ResetSelf();
            FinalTask.ResetSelf();
        }
    }
}