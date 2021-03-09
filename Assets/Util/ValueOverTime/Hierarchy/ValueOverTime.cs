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
        #region Property Fields

        private T _startValue;
        private T _endValue;

        #endregion Property Fields

        public abstract T Value { get; }

        public T StartValue
        {
            get => _startValue;
            set
            {
                _startValue = value;
                SetValueDifference();
            }
        }

        public T EndValue
        {
            get => _endValue;
            set
            {
                _endValue = value;
                SetValueDifference();
            }
        }
        protected T ValueDifference { get; private set; }

        public FrameTimer Timer { get; private set; }
        public float Duration => Timer.ActivationInterval;

        public ValueOverTime(T startValue, T endValue, float duration)
        {
            Timer = new FrameTimer(duration);
            InitValues(startValue, endValue);
        }

        private void InitValues(T startValue, T endValue)
        {
            _startValue = startValue;
            _endValue = endValue;
            SetValueDifference();
        }
        private void SetValueDifference()
        {
            ValueDifference = CalculateValueDifference();
        }

        protected abstract T CalculateValueDifference();

        protected virtual void OnIncrement(float deltaTime) { }
        public void Increment(float deltaTime)
        {
            Timer.Increment(deltaTime);
            OnIncrement(deltaTime);
        }

        public bool IsFinished => Timer.Activated;
    }
}
