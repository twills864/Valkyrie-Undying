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
    /// Modifies a time scale using the function y = x(2-x)
    /// Acts as a philosophical reversal of the EaseOut time scale.
    /// </summary>
    public class EaseIn : TimeModifyingGameTask
    {
        public EaseIn(FiniteTimeGameTask innerTask) : base(innerTask)
        {
        }

        protected override TimeModifyingFrameTimer DefaultTimeModifyingFrameTimer(float duration)
        {
            return new EaseInTimer(duration);
        }
    }
}
