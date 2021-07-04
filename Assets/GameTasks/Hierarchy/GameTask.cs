using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;
using LogUtilAssets;

namespace Assets.GameTasks
{
    /// <summary>
    /// Represents a task containing some form of common functionality
    /// that will be performed to a specified ValkyrieSprite.
    /// </summary>
    /// <inheritdoc/>
    public abstract class GameTask
    {
        /// <summary>
        /// The GameTaskRunner that will receive the effects of this GameTask.
        /// </summary>
        public ValkyrieSprite Target { get; }

        /// <summary>
        /// Whether or not this GameTask has finished running.
        /// </summary>
        public virtual bool IsFinished => Timer.Activated;

        /// <summary>
        /// The FrameTimer that will time the events of this GameTask.
        /// </summary>
        protected virtual FrameTimerBase Timer { get; set; }


        public GameTask(ValkyrieSprite target)
        {
            Target = target;
        }

        public abstract void RunFrame(float deltaTime);

        /// <summary>
        /// Runs a frame, and returns whether or not the task becomes finished as a result.
        /// </summary>
        /// <param name="deltaTime">The amount of time since the last frame.</param>
        /// <returns>Whether or not this task is finished.</returns>
        public bool FrameRunFinishes(float deltaTime)
        {
            RunFrame(deltaTime);
            return IsFinished;
        }

        /// <summary>
        /// Resets this task by resetting its timer.
        /// </summary>
        public virtual void ResetSelf()
        {
            Timer.Reset();
        }

        //protected abstract string DebugString();
        //public override string ToString()
        //{
        //    return DebugString();
        //}

        protected virtual string ToGameTaskString() => "";
        public sealed override string ToString()
        {
            return $"{GetType().Name} {Timer} {ToGameTaskString()}";
        }
    }
}
