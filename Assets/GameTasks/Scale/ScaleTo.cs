using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;
using UnityEngine;

namespace Assets.GameTasks
{
    /// <inheritdoc/>
    public class ScaleTo : FiniteTimeGameTask
    {
        private VectorValueOverTime ScaleValue { get; set; }

        public Vector3 StartValue
        {
            get => ScaleValue.StartValue;
            set => ScaleValue.StartValue = value;
        }

        public Vector3 EndValue
        {
            get => ScaleValue.EndValue;
            set => ScaleValue.EndValue = value;
        }

        protected override void OnDurationSet(float value)
        {
            // First time Duration gets set, ScaleValue will be null.
            if (ScaleValue != null)
                ScaleValue.Duration = value;
        }

        public ScaleTo(ValkyrieSprite target, float scale, float duration)
            : this(target, new Vector3(scale, scale, 1f), duration)
        {
        }

        public ScaleTo(ValkyrieSprite target, Vector3 scale, float duration) : base(target, duration)
        {
            var localScale = target.transform.localScale;
            ScaleValue = new VectorValueOverTime(localScale, scale, duration);
        }

        public ScaleTo(ValkyrieSprite target, float scaleStart, float scaleEnd, float duration)
            : this(target, new Vector3(scaleStart, scaleStart, 0), new Vector3(scaleEnd, scaleEnd, 0), duration)
        {
        }

        public ScaleTo(ValkyrieSprite target, Vector3 scaleStart, Vector3 scaleEnd, float duration) : base(target, duration)
        {
            ScaleValue = new VectorValueOverTime(scaleStart, scaleEnd, duration);
        }

        protected override void OnFiniteTaskFrameRun(float deltaTime)
        {
            ScaleValue.Timer.Elapsed = Timer.Elapsed;
            Target.transform.localScale = ScaleValue.Value;
        }

        protected override string ToFiniteTimeGameTaskString()
        {
            return $"{StartValue} -> {ScaleValue.Value} -> {EndValue}";
        }
    }
}
