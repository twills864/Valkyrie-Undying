using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
            return AdjustRatio(currentRatioComplete);
        }

        public static float AdjustRatio(float currentRatioComplete)
        {
            float ret = currentRatioComplete * currentRatioComplete;
            return ret;
        }

        // x = y^2
        // y = sqrt(x)
        public static float InverseRatio(float currentRatioComplete)
        {
            float ret = Mathf.Sqrt(currentRatioComplete);
            return ret;
        }
    }
}
