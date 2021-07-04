using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util
{
    /// <summary>
    /// A range of Vector2 values between a given start value
    /// and a given end value.
    /// </summary>
    /// <inheritdoc/>
    public struct Vector2Range : IValueRange<Vector2>
    {
        #region Property Fields

        private Vector2 _startValue;
        private Vector2 _endValue;

        #endregion Property Fields

        public Vector2 StartValue
        {
            get => _startValue;
            set
            {
                _startValue = value;
                SetRange();
            }
        }

        public Vector2 EndValue
        {
            get => _endValue;
            set
            {
                _endValue = value;
                SetRange();
            }
        }
        public Vector2 RangeDelta { get; private set; }

        public Vector2Range(Vector2 startValue, Vector2 endValue)
        {
            _startValue = startValue;
            _endValue = endValue;
            RangeDelta = _endValue - _startValue;
        }

        private void SetRange()
        {
            RangeDelta = EndValue - StartValue;
        }

        public Vector2 ValueAtRatio(float ratio)
        {
            Vector2 ret = StartValue + (ratio * RangeDelta);
            return ret;
        }
    }
}
