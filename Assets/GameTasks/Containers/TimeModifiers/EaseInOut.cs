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
    /// Modifies a time scale using the following Bézier curve:
    /// y = (x^2)(3 - 2x)
    /// </summary>
    /// <inheritdoc/>
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
