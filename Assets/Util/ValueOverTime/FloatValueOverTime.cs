using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public class FloatValueOverTime : ValueOverTime<float>
    {
        public override float Value => StartValue + (Timer.RatioComplete * ValueDifference);

        protected override float CalculateValueDifference() => EndValue - StartValue;

        public FloatValueOverTime(float start, float end, float duration) : base(start, end, duration)
        {
        }
    }
}
