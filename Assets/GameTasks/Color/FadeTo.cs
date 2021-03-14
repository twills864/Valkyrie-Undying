using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.GameTasks
{
    public class FadeTo : FiniteTimeGameTask
    {
        private float StartAlpha { get; set; }
        private float EndAlpha { get; set; }
        private float AlphaDifference { get; set; }

        public float Alpha
        {
            get => Target.Alpha;
            set => Target.Alpha = value;
        }

        public FadeTo(ValkyrieSprite target, float alpha, float duration) : base(target, duration)
        {
            StartAlpha = target.Alpha;
            EndAlpha = alpha;
            AlphaDifference = EndAlpha - StartAlpha;

            //Alpha = alpha;
        }

        protected override void OnFiniteTaskFrameRun(float deltaTime)
        {
            Alpha = StartAlpha + (Timer.RatioComplete * AlphaDifference);
        }
    }
}
