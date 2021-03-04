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
    /// Modifies a time scale using the function y = x^3.
    /// </summary>
    public class EaseOut3Timer : TimeModifyingFrameTimer
    {
        public EaseOut3Timer(float activationInterval) : base(activationInterval)
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

        // y = x^3
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AdjustRatio(float currentRatioComplete)
        {
            float ret = currentRatioComplete * currentRatioComplete * currentRatioComplete;
            return ret;
        }

        // x = y^3
        // y = cuberoot(x)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float InvertRatio(float currentRatioComplete)
        {
            float ret = Mathf.Pow(currentRatioComplete, 1f / 3);
            return ret;
        }
    }
}
