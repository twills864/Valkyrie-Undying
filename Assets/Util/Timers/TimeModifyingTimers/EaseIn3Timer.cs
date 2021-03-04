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
    /// Modifies a time scale using the function y = 1 + (x-1)^3
    /// Acts as a philosophical reversal of the EaseOut3Timer.
    /// </summary>
    public class EaseIn3Timer : TimeModifyingFrameTimer
    {
        public EaseIn3Timer(float activationInterval) : base(activationInterval)
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

        // y = 1 + (x-1)^3
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AdjustRatio(float currentRatioComplete)
        {
            float cubed = (currentRatioComplete - 1);
            cubed *= cubed * cubed;
            float ret = 1 + cubed;
            return ret;
        }

        // x = 1 + (y-1)^3
        // y = 1 + cuberoot(x - 1)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InvertRatio(float currentRatioComplete)
        {
            float ret = 1 + Mathf.Pow(currentRatioComplete - 1, 1f / 3);
            return ret;
        }
    }
}
