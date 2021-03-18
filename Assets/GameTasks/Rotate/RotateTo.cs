using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;
using UnityEngine;

namespace Assets.GameTasks
{
    public class RotateTo : FiniteTimeGameTask
    {
        private FloatRange AngleRange;

        private float LastAngle;

        public float RotationDegrees
        {
            get => Target.RotationDegrees;
            set => Target.RotationDegrees = value;
        }

        public RotateTo(ValkyrieSprite target, float endAngle, float duration)
            : this(target, target.RotationDegrees, endAngle, duration)
        {
        }

        public RotateTo(ValkyrieSprite target, float startAngle, float endAngle, float duration) : base(target, duration)
        {
            AngleRange = new FloatRange(startAngle, endAngle);

            LastAngle = startAngle;
        }

        protected override void OnFiniteTaskFrameRun(float deltaTime)
        {
            float newAngle = AngleRange.ValueAtRatio(Timer.RatioComplete);
            //float angleDifference = newAngle - LastAngle;

            RotationDegrees = newAngle;
            //Target.RotateSprite(angleDifference);
            LastAngle = newAngle;
        }

        public override void ResetSelf()
        {
            base.ResetSelf();
            LastAngle = AngleRange.StartValue;
        }

        protected override string ToFiniteTimeGameTaskString()
        {
            return $"{AngleRange.StartValue} -> {RotationDegrees} -> {AngleRange.EndValue}";
        }
    }
}
