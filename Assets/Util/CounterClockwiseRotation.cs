using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public struct CounterClockwiseRotation
    {
        public float Angle { get; private set; }

        public void AddAngle(float angle)
        {
            Angle += angle;
            FixAngle();
        }

        private void FixAngle()
        {
            if (Angle >= MathUtil.Pi2f)
                Angle -= MathUtil.Pi2f;
        }

        public static implicit operator float(CounterClockwiseRotation rotation) => rotation.Angle;
    }
}
