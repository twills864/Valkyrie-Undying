using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public class VectorValueOverTime
    {
        private Vector3 StartValue { get; set; }
        private Vector3 EndValue { get; set; }
        private Vector3 ValueDifference { get; set; }

        private FrameTimer Timer { get; set; }

        public VectorValueOverTime(Vector3 start, Vector3 end, float duration)
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

        public Vector3 Value => StartValue + (Timer.RatioComplete * ValueDifference);

        public float RatioComplete => Timer.RatioComplete;
        public float RatioRemaining => Timer.RatioRemaining;
        public float TimeUntilActivation => Timer.TimeUntilActivation;
        public float Elapsed => Timer.Elapsed;
    }
}
