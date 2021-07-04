using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    /// <summary>
    /// A float value that will grow from a specified start value to a
    /// specified end value over a specified amount of time.
    /// </summary>
    /// <inheritdoc/>
    public class FloatValueOverTime : ValueOverTime<float>
    {
        public override float Value => StartValue + (Timer.RatioComplete * ValueDifference);

        protected override float CalculateValueDifference() => EndValue - StartValue;

        public FloatValueOverTime(float start, float end, float duration) : base(start, end, duration)
        {
        }
    }
}
