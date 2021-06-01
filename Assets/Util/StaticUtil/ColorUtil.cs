using System;
using UnityEngine;

namespace Assets.Util
{
    public static class ColorUtil
    {
        /// <summary>
        /// Returns a Color with identical r. g, and b values to a given <paramref name="input"/>,
        /// but with the alpha set to a specified value.
        /// </summary>
        /// <param name="input">The input Color.</param>
        /// <param name="alpha">The alpha value to set.</param>
        /// <returns>The same color, but with the specified alpha.</returns>
        public static Color WithAlpha(Color input, float alpha)
        {
            input.a = alpha;
            return input;
        }

        /// <summary>
        /// Returns a Color with identical r. g, and b values to a given <paramref name="input"/>,
        /// but with the alpha set to its maximum opaqueness.
        /// </summary>
        /// <param name="input">The input Color.</param>
        /// <returns>The same color, but fully opaque.</returns>
        public static Color Opaque(Color input)
        {
            const float FullyOpaque = 1.0f;
            input.a = FullyOpaque;
            return input;
        }
    }
}
