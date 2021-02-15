using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameTasks
{
    /// <summary>
    /// Modifies a time scale using the function y = x^2.
    /// </summary>
    public class EaseOut : TimeModifyingGameTask
    {
        public EaseOut(FiniteTimeGameTask innerTask) : base(innerTask)
        {
        }

        protected override float ModifyCompletionRatio(float currentRatioComplete)
        {
            return currentRatioComplete * currentRatioComplete;
        }
    }
}
