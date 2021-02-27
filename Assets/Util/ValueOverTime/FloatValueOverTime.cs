using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public class FloatValueOverTime
    {
        private float StartValue { get; set; }
        private float EndValue { get; set; }
        private float ValueDifference { get; set; }

        private FrameTimer Timer { get; set; }

        public FloatValueOverTime(float start, float end, float duration)
        {
            StartValue = start;
            EndValue = end;
            ValueDifference = EndValue - StartValue;

            Timer = new FrameTimer(duration);
        }

        public void Increment(float deltaTime)
        {
            Timer.Increment(deltaTime);
        }

        public float Value => StartValue + (Timer.RatioComplete * ValueDifference);

        public float RatioComplete => Timer.RatioRemaining;
        public float RatioRemaining => Timer.RatioRemaining;
        public float TimeUntilActivation => Timer.TimeUntilActivation;
        public float Elapsed => Timer.Elapsed;
    }
}
