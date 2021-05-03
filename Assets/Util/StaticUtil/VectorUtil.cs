using System;
using UnityEngine;

namespace Assets.Util
{
    public static class VectorUtil
    {
        /// <summary>
        /// Returns a Vector3 with identical Y and Z values to a given <paramref name="input"/>,
        /// but with the X set to a specified value.
        /// </summary>
        /// <param name="input">The input Vector3.</param>
        /// <param name="x">The X value to set.</param>
        /// <returns></returns>
        public static Vector3 WithX(Vector3 input, float x)
        {
            input.x = x;
            return input;
        }

        /// <summary>
        /// Returns a Vector3 with identical X and Z values to a given <paramref name="input"/>,
        /// but with the Y set to a specified value.
        /// </summary>
        /// <param name="input">The input Vector3.</param>
        /// <param name="y">The Y value to set.</param>
        /// <returns></returns>
        public static Vector3 WithY(Vector3 input, float y)
        {
            input.y = y;
            return input;
        }


        /// <summary>
        /// Returns a Vector3 with identical Y and Z values to a given <paramref name="input"/>,
        /// but with an X value equal to <paramref name="input"/>.x * <paramref name="xScale"/>.
        /// </summary>
        /// <param name="input">The input Vector3.</param>
        /// <param name="xScale">The value to scale input.x by.</param>
        /// <returns></returns>
        public static Vector3 ScaleX(Vector3 input, float xScale)
        {
            input.x *= xScale;
            return input;
        }

        /// <summary>
        /// Returns a Vector3 with identical X and Z values to a given <paramref name="input"/>,
        /// but with a Y value equal to <paramref name="input"/>.y * <paramref name="yScale"/>.
        /// </summary>
        /// <param name="input">The input Vector3.</param>
        /// <param name="yScale">The value to scale input.y by.</param>
        /// <returns></returns>
        public static Vector3 ScaleY(Vector3 input, float yScale)
        {
            input.y *= yScale;
            return input;
        }


        /// <summary>
        /// Returns a Vector3 with identical Y and Z values to a given <paramref name="input"/>,
        /// but with an X value equal to <paramref name="input"/>.x + <paramref name="xAdd"/>.
        /// </summary>
        /// <param name="input">The input Vector3.</param>
        /// <param name="xAdd">The value to add to input.x.</param>
        /// <returns></returns>
        public static Vector3 AddX(Vector3 input, float xAdd)
        {
            input.x += xAdd;
            return input;
        }

        /// <summary>
        /// Returns a Vector3 with identical X and Z values to a given <paramref name="input"/>,
        /// but with a Y value equal to <paramref name="input"/>.y + <paramref name="yAdd"/>.
        /// </summary>
        /// <param name="input">The input Vector3.</param>
        /// <param name="yAdd">The value to add to input.y.</param>
        /// <returns></returns>
        public static Vector3 AddY(Vector3 input, float yAdd)
        {
            input.y += yAdd;
            return input;
        }
    }
}
