using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public struct FloatRange : IValueRange<float>
    {
        #region Property Fields

        private float _startValue;
        private float _endValue;

        #endregion Property Fields

        public float StartValue
        {
            get => _startValue;
            set
            {
                _startValue = value;
                SetRange();
            }
        }

        public float EndValue
        {
            get => _endValue;
            set
            {
                _endValue = value;
                SetRange();
            }
        }
        public float RangeDelta { get; private set; }

        public FloatRange(float startValue, float endValue)
        {
            _startValue = startValue;
            _endValue = endValue;
            RangeDelta = _endValue - _startValue;
        }

        public void SetRange(float startValue, float endValue)
        {
            _startValue = startValue;
            _endValue = endValue;
            SetRange();
        }

        private void SetRange()
        {
            RangeDelta = EndValue - StartValue;
        }

        public float ValueAtRatio(float ratio)
        {
            float ret = StartValue + (ratio * RangeDelta);
            return ret;
        }
    }
}
