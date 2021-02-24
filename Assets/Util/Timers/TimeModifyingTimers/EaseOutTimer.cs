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
    /// Modifies a time scale using the function y = x^2.
    /// </summary>
    public class EaseOutTimer : TimeModifyingFrameTimer
    {
        public EaseOutTimer(float activationInterval) : base(activationInterval)
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

        // y = x^2
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AdjustRatio(float currentRatioComplete)
        {
            float ret = currentRatioComplete * currentRatioComplete;
            return ret;
        }

        // x = y^2
        // y = sqrt(x)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InvertRatio(float currentRatioComplete)
        {
            float ret = Mathf.Sqrt(currentRatioComplete);
            return ret;
        }
    }
}
