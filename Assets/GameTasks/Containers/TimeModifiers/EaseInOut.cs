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
    /// Modifies a time scale using the following piecewise-function:
    /// x <= 0.5: y = 2(x^2)
    /// x  > 0.5: y = 1 - ([([-2 * x] + 2) ^ 2] / 2)
    /// </summary>
    public class EaseInOut : TimeModifyingGameTask
    {
        public EaseInOut(FiniteTimeGameTask innerTask) : base(innerTask)
        {
        }

        protected override TimeModifyingFrameTimer DefaultTimeModifyingFrameTimer(float duration)
        {
            return new EaseInOutTimer(duration);
        }
    }
}
