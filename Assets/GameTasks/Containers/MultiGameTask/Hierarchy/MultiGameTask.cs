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
    /// Runs several different game tasks in a subclass-specified fashion.
    /// </summary>
    /// <inheritdoc/>
    public abstract class MultiGameTask : FiniteTimeGameTask
    {
        protected FiniteTimeGameTask[] InnerTasks { get; set; }

        public MultiGameTask(ValkyrieSprite target, float duration, params FiniteTimeGameTask[] innerTasks)
            : base(target, duration)
        {
            InnerTasks = innerTasks;
        }

        protected virtual void OnMultiGameTaskReset() { }
        public sealed override void ResetSelf()
        {
            base.ResetSelf();

            OnMultiGameTaskReset();
        }

        protected void ResetInnerTasks()
        {
            foreach (var task in InnerTasks)
                task.ResetSelf();
        }
    }
}