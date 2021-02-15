using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.GameTasks
{
    /// <summary>
    /// Modifies a time scale using the function y = 1 - (1-x)^2.
    /// Acts as a philosophical reversal of the EaseOut time scale.
    /// </summary>
    public class EaseIn : TimeModifyingGameTask
    {
        public EaseIn(FiniteTimeGameTask innerTask) : base(innerTask)
        {
        }

        protected override float ModifyCompletionRatio(float currentRatioComplete)
        {
            float x = 1 - currentRatioComplete;
            float squared = x * x;

            float ret = 1-squared;
            return ret;
        }
    }
}
