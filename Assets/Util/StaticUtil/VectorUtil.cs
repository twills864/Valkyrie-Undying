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
    }
}
