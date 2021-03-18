using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;
using UnityEngine;

namespace Assets.GameTasks
{
    public class FadeTo : FiniteTimeGameTask
    {
        private FloatRange AlphaRange;

        public float Alpha
        {
            get => Target.Alpha;
            set => Target.Alpha = value;
        }

        public FadeTo(ValkyrieSprite target, float endAlpha, float duration)
            : this(target, target.Alpha, endAlpha, duration)
        {
        }

        public FadeTo(ValkyrieSprite target, float startAlpha, float endAlpha, float duration) : base(target, duration)
        {
            AlphaRange = new FloatRange(startAlpha, endAlpha);
        }

        protected override void OnFiniteTaskFrameRun(float deltaTime)
        {
            Alpha = AlphaRange.ValueAtRatio(Timer.RatioComplete);
        }

        protected override string ToFiniteTimeGameTaskString()
        {
            return $"{AlphaRange.StartValue} -> {Alpha} -> {AlphaRange.EndValue}";
        }
    }
}
