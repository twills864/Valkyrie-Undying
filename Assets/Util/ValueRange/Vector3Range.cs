using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util
{
    public struct Vector3Range : IValueRange<Vector3>
    {
        #region Property Fields

        private Vector3 _startValue;
        private Vector3 _endValue;

        #endregion Property Fields

        public Vector3 StartValue
        {
            get => _startValue;
            set
            {
                _startValue = value;
                SetRange();
            }
        }

        public Vector3 EndValue
        {
            get => _endValue;
            set
            {
                _endValue = value;
                SetRange();
            }
        }
        public Vector3 RangeDelta { get; private set; }

        public Vector3Range(Vector3 startValue, Vector3 endValue)
        {
            _startValue = startValue;
            _endValue = endValue;
            RangeDelta = _endValue - _startValue;
        }

        private void SetRange()
        {
            RangeDelta = EndValue - StartValue;
        }

        public Vector3 ValueAtRatio(float ratio)
        {
            Vector3 ret = StartValue + (ratio * RangeDelta);
            return ret;
        }
    }
}
