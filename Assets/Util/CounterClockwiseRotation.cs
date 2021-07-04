using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    /// <summary>
    /// Represents a rotation in radians that should exist between 0 and 2pi.
    /// </summary>
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
