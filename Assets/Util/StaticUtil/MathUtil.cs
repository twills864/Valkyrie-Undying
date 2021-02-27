using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Util
{
    public static class MathUtil
    {
        public const float Pi2f = Mathf.PI * 2f;
        public const float PiHalf = Mathf.PI * 0.5f;
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
        /// Raises a given floating point number to a given integer <paramref name="exponent"/>.
        /// Exponentiation by squaring has not yet proven to be consistently
        /// and significantly faster than Mathf.Pow().
        /// Therefore, Mathf.Pow() is used instead.
        /// </summary>
        /// <param name="baseNumber">The number to raise to a given exponent.</param>
        /// <param name="exponent">The exponent to raise a given number to.</param>
        /// <returns>The given number raised to the given exponent.</returns>
        /// <seealso cref="https://gist.github.com/riyadparvez/5915147"/>
        public static float Pow(float baseNumber, int exponent)
        {
            float ret = Mathf.Pow(baseNumber, exponent);
            return ret;

            #region Deprecated Exponentiation by Squaring
            // Exponentiation by squaring code below, adapted from:
            // https://gist.github.com/riyadparvez/5915147

            //float result = 1.0f;
            //while (exponent > 0)
            //{
            //    if (IsOdd(exponent))
            //        result *= baseNumber;
            //    exponent >>= 1;
            //    baseNumber *= baseNumber;
            //}

            //return result;
            #endregion Deprecated Exponentiation by Squaring
        }

        #region Even / Odd

        /// <summary>
        /// Returns true if a given <paramref name="number"/> is even; false otherwise;
        /// </summary>
        /// <param name="number">The given number.</param>
        /// <returns>True if the given <paramref name="number"/> is even; false otherwise;</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEven(int number)
        {
            bool ret = (number & 1) == 0;
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
            bool ret = (number & 1) == 1;
            return ret;
        }

        #endregion Even / Odd

        #region Vector

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

        /// <summary>
        /// Returns a unit vector pointed at a given <paramref name="angle"/>.
        /// </summary>
        /// <param name="angle">The angle at which to point the vector.</param>
        /// <returns>The unit vector at the given angle.</returns>
        public static Vector2 VectorAtAngle(float angle)
        {
            var x = Mathf.Cos(angle);
            var y = Mathf.Sin(angle);

            var ret = new Vector2(x, y);
            return ret;
        }

        /// <summary>
        /// Returns a vector of a given length pointed at a given <paramref name="angle"/>.
        /// </summary>
        /// <param name="angle">The angle at which to point the vector.</param>
        /// <returns>The vector at the given angle.</returns>
        public static Vector2 VectorAtAngle(float angle, float length)
        {
            var ret = VectorAtAngle(angle) * length;
            return ret;
        }

        /// <summary>
        /// Returns a Vector2 along the path between two points
        /// that represents the given ration between them.
        /// </summary>
        /// <param name="from">The starting point.</param>
        /// <param name="to">The destination point.</param>
        /// <param name="ratio">The ratio of completion.</param>
        /// <returns>The represented point between the two points.</returns>
        public static Vector2 ScaledPositionBetween(Vector2 from, Vector2 to, float ratio)
        {
            Vector2 diff = to - from;
            Vector2 scaled = diff * ratio;

            Vector2 ret = from + scaled;
            return ret;
        }

        /// <summary>
        /// Returns a Vector3 along the path between two points
        /// that represents the given ration between them.
        /// </summary>
        /// <param name="from">The starting point.</param>
        /// <param name="to">The destination point.</param>
        /// <param name="ratio">The ratio of completion.</param>
        /// <returns>The represented point between the two points.</returns>
        public static Vector3 ScaledPositionBetween(Vector3 from, Vector3 to, float ratio)
        {
            Vector3 diff = to - from;
            Vector3 scaled = diff * ratio;

            Vector3 ret = from + scaled;
            return ret;
        }

        #endregion Vector

        #region Quaternion

        public static Quaternion RotationToQuaternion(float angle)
        {
            return RotationToQuaternion(new Vector3(0, 0, angle));
        }
        public static Quaternion RotationToQuaternion(Vector3 rotation)
        {
            var ret = Quaternion.identity;
            ret.eulerAngles = rotation;

            return ret;
        }

        #endregion Quaternion
    }
}
