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


        /// <summary>
        /// Returns true if a given <paramref name="number"/> is even; false otherwise;
        /// </summary>
        /// <param name="number">The given number.</param>
        /// <returns>True if the given <paramref name="number"/> is even; false otherwise;</returns>
        public static bool IsEven(int number)
        {
            bool ret = number % 2 == 0;
            return ret;
        }

        /// <summary>
        /// Returns true if a given <paramref name="number"/> is odd; false otherwise;
        /// </summary>
        /// <param name="number">The given number.</param>
        /// <returns>True if the given <paramref name="number"/> is odd; false otherwise;</returns>
        public static bool IsOdd(int number)
        {
            return !IsEven(number);
        }
    }
}
