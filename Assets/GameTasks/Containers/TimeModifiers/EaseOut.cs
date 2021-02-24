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
    /// Modifies a time scale using the function y = x^2.
    /// </summary>
    public class EaseOut : TimeModifyingGameTask
    {
        public EaseOut(FiniteTimeGameTask innerTask) : base(innerTask)
        {
        }

        protected override TimeModifyingFrameTimer DefaultTimeModifyingFrameTimer(float duration)
        {
            return new EaseOutTimer(duration);
        }
    }
}
