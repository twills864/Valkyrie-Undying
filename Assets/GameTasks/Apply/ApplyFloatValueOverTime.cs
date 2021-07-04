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
    /// Applies the current value of a given FloatValueOverTime using custom specified Action.
    /// </summary>
    /// <inheritdoc/>
    public class ApplyFloatValueOverTime : FiniteTimeGameTask
    {
        private FloatValueOverTime Range { get; set; }
        private Action<float> Action { get; set; }

        public float CurrentValue => Range.Value;

        public ApplyFloatValueOverTime(ValkyrieSprite target, Action<float> action, float startValue, float endValue, float duration) : base(target, duration)
        {
            Action = action;
            Range = new FloatValueOverTime(startValue, endValue, duration);
        }

        protected override void OnFiniteTaskFrameRun(float deltaTime)
        {
            Range.Increment(deltaTime);
            Action(CurrentValue);
        }
    }
}