using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameTasks
{
    public class RotateTo : FiniteTimeGameTask
    {
        private float StartAngle;
        private float EndAngle;
        private float AngleDifference;

        private float LastAngle;

        //public float Rotation
        //{
        //    get => Target.Rotation;
        //    set => Target.Rotation = value;
        //}

        public RotateTo(ValkyrieSprite target, float endAngle, float duration)
            : this(target, target.RotationDegrees, endAngle, duration)
        {
        }

        public RotateTo(ValkyrieSprite target, float startAngle, float endAngle, float duration) : base(target, duration)
        {
            StartAngle = startAngle;
            EndAngle = endAngle;
            AngleDifference = EndAngle - StartAngle;

            LastAngle = StartAngle;
        }

        protected override void OnFiniteTaskFrameRun(float deltaTime)
        {
            float newAngle = StartAngle + (Timer.RatioComplete * AngleDifference);
            float angleDifference = newAngle - LastAngle;
            Target.RotateSprite(angleDifference);
            LastAngle = newAngle;
        }

        public override void ResetSelf()
        {
            base.ResetSelf();
            LastAngle = StartAngle;
        }
    }
}
