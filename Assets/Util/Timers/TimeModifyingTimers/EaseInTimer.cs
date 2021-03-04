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
    /// Modifies a time scale using the function y = x(2-x)
    /// Acts as a philosophical reversal of the EaseOutTimer.
    /// </summary>
    public class EaseInTimer : TimeModifyingFrameTimer
    {
        public EaseInTimer(float activationInterval) : base(activationInterval)
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

        // y = x(2-x)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AdjustRatio(float currentRatioComplete)
        {
            float ret = currentRatioComplete * (2 - currentRatioComplete);
            return ret;
        }

        // x = y(2-y)
        // y = 1 ± sqrt(-x + 1)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InvertRatio(float currentRatioComplete)
        {
            float ret = 1 - Mathf.Sqrt(-currentRatioComplete + 1);
            return ret;
        }
    }
}
