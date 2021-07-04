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
    /// Rotates the target ValkyrieSprite to a specified angle in degrees
    /// over a specified period of time.
    /// </summary>
    /// <inheritdoc/>
    public class RotateTo : FiniteTimeGameTask
    {
        private FloatRange AngleRange;

        private float LastAngle;

        private float RotationDegrees
        {
            get => Target.RotationDegrees;
            set => Target.RotationDegrees = value;
        }

        public float StartRotationDegrees
        {
            get => AngleRange.StartValue;
            set => AngleRange.StartValue = value;
        }

        public float EndRotationDegrees
        {
            get => AngleRange.EndValue;
            set => AngleRange.EndValue = value;
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

        public void SetAngleRange(float from, float to)
        {
            AngleRange.StartValue = from;
            AngleRange.EndValue = to;
        }

        public void SetMinimumArcAngleRange(float from, float to)
        {
            const float FullCircle = 360f;
            const float HalfCircle = 180f;

            if (Mathf.Abs(from - to) > FullCircle)
            {
                if (from < 0)
                    from += FullCircle;
                else
                    to += FullCircle;
            }

            if (Mathf.Abs(from - to) > HalfCircle)
            {
                if (from > to)
                    from -= FullCircle;
                else
                    to -= FullCircle;
            }

            AngleRange.SetRange(from, to);
        }

        public override void ResetSelf()
        {
            base.ResetSelf();
            LastAngle = AngleRange.StartValue;
        }

        public static RotateTo Default(ValkyrieSprite target, float duration)
        {
            var ret = new RotateTo(target, target.RotationDegrees, duration);
            ret.FinishSelf();
            return ret;
        }

        protected override string ToFiniteTimeGameTaskString()
        {
            return $"{AngleRange.StartValue} -> {RotationDegrees} -> {AngleRange.EndValue}";
        }
    }
}
