using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Assets.GameTasks;
using UnityEngine;

namespace Assets.Util
{
    /// <summary>
    /// Modifies a time scale using the following piecewise-function:
    /// x <= 0.5: y = 2(x^2)
    /// x  > 0.5: y = 1 - ([([-2 * x] + 2) ^ 2] / 2)
    /// </summary>
    public class EaseInOutTimer : TimeModifyingFrameTimer
    {
        public EaseInOutTimer(float activationInterval) : base(activationInterval)
        {
        }

        protected override float Adjust(float currentRatioComplete)
        {
            return AdjustRatio(currentRatioComplete);
        }

        protected override float Invert(float currentRatioComplete)
        {
            return InvertRatio(currentRatioComplete);
        }

        // x <= 0.5: y = 2(x^2)
        // x  > 0.5: y = 1 - ([([-2 * x] + 2) ^ 2] / 2)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AdjustRatio(float currentRatioComplete)
        {
            float ret;
            if(currentRatioComplete < 0.5f)
                ret = 2 * currentRatioComplete * currentRatioComplete;
            else
            {
                float numerator = (-2f * currentRatioComplete) + 2;
                numerator *= numerator;

                ret = 1 - (0.5f * numerator);
            }

            return ret;
        }

        // x <= 0.5: x = 2(y^2)
        //           y = sqrt(x/2)
        // x  > 0.5: x = 1 - ([([-2 * y] + 2) ^ 2] / 2)
        //           y = 1 + (-sqrt(-2x+2)/2)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InvertRatio(float currentRatioComplete)
        {
            float ret;
            if (currentRatioComplete < 0.5f)
                ret = Mathf.Sqrt(currentRatioComplete * 0.5f);
            else
            {
                float sqrt = -Mathf.Sqrt((-2f * currentRatioComplete) + 2f);
                ret = 1 + (sqrt * 0.5f);
            }

            return ret;
        }
    }
}
