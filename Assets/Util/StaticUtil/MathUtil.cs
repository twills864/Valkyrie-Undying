using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public static class MathUtil
    {
        /// <summary>
        /// Calculates the result of <paramref name="dividen"/> modulo <paramref name="divisor"/>.
        /// </summary>
        /// <returns>The result of <paramref name="dividen"/> mod <paramref name="divisor"/></returns>
        public static int Mod(int dividen, int divisor)
        {
            int ret = (dividen % divisor + divisor) % divisor;
            return ret;
        }
    }
}
