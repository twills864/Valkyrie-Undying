using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util
{
    public class VectorValueOverTime : ValueOverTime<Vector3>
    {
        public override Vector3 Value => StartValue + (Timer.RatioComplete * ValueDifference);

        protected override Vector3 CalculateValueDifference() => EndValue - StartValue;

        public VectorValueOverTime(Vector3 start, Vector3 end, float duration)
            : base(start, end, duration)
        {
        }
    }
}
