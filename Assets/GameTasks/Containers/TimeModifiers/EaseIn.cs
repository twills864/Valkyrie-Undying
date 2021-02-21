using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameTasks
{
    /// <summary>
    /// Modifies a time scale using the function y = x(2-x)
    /// Acts as a philosophical reversal of the EaseOut time scale.
    /// </summary>
    public class EaseIn : TimeModifyingGameTask
    {
        public EaseIn(FiniteTimeGameTask innerTask) : base(innerTask)
        {
        }

        protected override float ModifyCompletionRatio(float currentRatioComplete)
        {
            return AdjustRatio(currentRatioComplete);
        }

        public static float AdjustRatio(float currentRatioComplete)
        {
            float ret = currentRatioComplete * (2 - currentRatioComplete);
            return ret;
        }

        // x = y(2-y)
        // y = 1 ± sqrt(-x + 1)
        public static float InverseRatio(float currentRatioComplete)
        {
            float ret = 1 - Mathf.Sqrt(-currentRatioComplete + 1);
            return ret;
        }

    }
}
