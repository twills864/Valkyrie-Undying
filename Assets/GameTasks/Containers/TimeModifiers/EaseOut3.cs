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
    /// Modifies a time scale using the function y = x^3.
    /// </summary>
    /// <inheritdoc/>
    public class EaseOut3 : TimeModifyingGameTask
    {
        public EaseOut3(FiniteTimeGameTask innerTask) : base(innerTask)
        {
        }

        protected override TimeModifyingFrameTimer DefaultTimeModifyingFrameTimer(float duration)
        {
            return new EaseOut3Timer(duration);
        }
    }
}
