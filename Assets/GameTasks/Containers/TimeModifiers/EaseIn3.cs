using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;
using UnityEngine;

namespace Assets.GameTasks
{
    /// <summary>
    /// Modifies a time scale using the function y = 1 + (x-1)^3
    /// Acts as a philosophical reversal of the EaseOut3 time scale.
    /// </summary>
    /// <inheritdoc/>
    public class EaseIn3 : TimeModifyingGameTask
    {
        public EaseIn3(FiniteTimeGameTask innerTask) : base(innerTask)
        {
        }

        protected override TimeModifyingFrameTimer DefaultTimeModifyingFrameTimer(float duration)
        {
            return new EaseIn3Timer(duration);
        }
    }
}
