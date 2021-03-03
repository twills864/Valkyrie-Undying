using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util
{
    public abstract class ValueOverTime<T> where T : struct
    {
        public abstract T Value { get; }

        protected T StartValue { get; set; }
        protected T EndValue { get; set; }
        protected T ValueDifference { get; set; }

        public FrameTimer Timer { get; private set; }
        public float Duration => Timer.ActivationInterval;

        public ValueOverTime(T startValue, T endValue, float duration)
        {
            StartValue = startValue;
            EndValue = endValue;
            ValueDifference = CalculateValueDifference();

            Timer = new FrameTimer(duration);
        }

        protected abstract T CalculateValueDifference();

        protected virtual void OnIncrement(float deltaTime) { }
        public void Increment(float deltaTime)
        {
            Timer.Increment(deltaTime);
            OnIncrement(deltaTime);
        }


    }
}
