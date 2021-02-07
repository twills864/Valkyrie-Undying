using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Util
{
    public static class MathUtil
    {
        /// <summary>
        /// Calculates the result of <paramref name="dividen"/> modulo <paramref name="divisor"/>.
        /// </summary>
        /// <returns>The result of <paramref name="dividen"/> mod <paramref name="divisor"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOdd(int number)
        {
            return !IsEven(number);
        }


        /// <summary>
        /// Calculates the vector between two points, then normalizes it to a given velocity.
        /// </summary>
        /// <param name="from">The source from which to start the vector measurement.</param>
        /// <param name="to">The destination of the vector measurement.</param>
        /// <param name="velocity">The velocity to give to the normalized direction vector.</param>
        /// <returns></returns>
        public static Vector2 VelocityVector(Vector2 from, Vector2 to, float velocity = 1.0f)
        {
            Vector2 ret = to - from;
            ret.Normalize();
            ret *= velocity;
            return ret;
        }
    }
}
