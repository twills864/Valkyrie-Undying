using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util
{
    /// <summary>
    /// Represents a range of values between a given start value
    /// and a given end value.
    /// </summary>
    /// <typeparam name="T">The type of range to represent.</typeparam>
    public abstract class ValueRange<T> where T : struct
    {
        #region Property Fields

        private T _startValue;
        private T _endValue;

        #endregion Property Fields

        public abstract T ValueAtRatio(float ratio);
        protected abstract T CalculateRange();

        public T StartValue
        {
            get => _startValue;
            set
            {
                _startValue = value;
                SetRange();
            }
        }

        public T EndValue
        {
            get => _endValue;
            set
            {
                _endValue = value;
                SetRange();
            }
        }
        public T Range { get; private set; }

        public ValueRange(T startValue, T endValue)
        {
            _startValue = startValue;
            _endValue = endValue;
            SetRange();
        }

        private void SetRange()
        {
            Range = CalculateRange();
        }
    }
}
