using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Util
{
    public static class MathUtil
    {
        /// <summary>
        /// The default direction to apply if a sensitive Vector calculation results in Vector2.zero
        /// </summary>
        public static Vector2 DefaultVector => Vector2.up;

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
        /// Returns MathUtil.DefaultVector if a specified <paramref name="vector"/> is equal to Vector2.zero.
        /// </summary>
        /// <param name="vector">The vector to compare3 to Vector2.zero.</param>
        /// <returns>The original vector if it is not equal to Vector2.zero; MathUtil.DefaultVector2.zero otherwide.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 DefaultVectorIfZero(Vector2 vector)
        {
            var ret = vector != Vector2.zero ? vector : DefaultVector;
            return ret;
        }


        /// <summary>
        /// Normalizes a velocity vector with the given directional speeds to a given velocity.
        /// </summary>
        /// <param name="velocityX">The X speed of the velocity vector.</param>
        /// <param name="velocityY">The Y speed of the velocity vector.</param>
        /// <param name="velocity">The velocity to give to the normalized direction vector.</param>
        /// <returns></returns>
        public static Vector2 VelocityVector(float velocityX, float velocityY, float velocity = 1.0f)
        {
            var velocityVector = new Vector2(velocityX, velocityY);
            var ret = VelocityVector(velocityVector, velocity);
            return ret;
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
            var ret = VelocityVector(to - from, velocity);
            return ret;
        }

        /// <summary>
        /// Normalizes a given velocity vector to a given velocity.
        /// </summary>
        /// <param name="velocityVector">The velocity vector to normalize.</param>
        /// <param name="velocity">The velocity to give to the normalized direction vector.</param>
        /// <returns></returns>
        public static Vector2 VelocityVector(Vector2 velocityVector, float velocity = 1.0f)
        {
            var ret = velocityVector;

            if (ret == Vector2.zero)
                ret = DefaultVector;
            else
                ret.Normalize();

            ret *= velocity;
            return ret;
        }
    }
}
