using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

using Random = System.Random;

namespace Assets.Util
{
    /// <summary>
    /// Contains useful methods for generating or evaluating random results
    /// that need to be biased towards a certain value.
    /// </summary>
    /// <inheritdoc />
    public static class WeightedRandomUtil
    {
        private const float RatioClamp = 0.999f;

        #region Float

        #region Top-Weighted

        /// <summary>
        /// Returns a random float [0, 1).
        /// Higher values are more likely to be returned than lower ones.
        /// </summary>
        /// <returns>A top-weighted random float [0, 1).</returns>
        public static float FloatTopWeighted()
        {
            const float MinInclusive = 0.0f;
            const float MaxExclusive = 1.0f;

            return FloatTopWeighted(MinInclusive, MaxExclusive);
        }

        /// <summary>
        /// Returns a random float [0, <paramref name="maxExclusive"/>).
        /// Higher values are more likely to be returned than lower ones.
        /// </summary>
        /// <returns>A top-weighted random float [0, <paramref name="maxExclusive"/>).</returns>
        public static float FloatTopWeighted(float maxExclusive)
        {
            const float MinInclusive = 0.0f;
            return FloatTopWeighted(MinInclusive, maxExclusive);
        }

        /// <summary>
        /// Returns a random float [<paramref name="minInclusive"/>, <paramref name="maxExclusive"/>).
        /// Higher values are more likely to be returned than lower ones.
        /// </summary>
        /// <returns>A top-weighted random float [<paramref name="minInclusive"/>, <paramref name="maxExclusive"/>).</returns>
        public static float FloatTopWeighted(float minInclusive, float maxExclusive)
        {
            float lowerBound = RandomUtil.Float(minInclusive, maxExclusive);
            float upperBound = RandomUtil.Float(lowerBound, maxExclusive);

            float value = RandomUtil.Float(lowerBound, upperBound);

            // Prevent rounding errors from including maxExclusive as a valid result.
            value *= RatioClamp;
            return value;
        }

        #endregion Top-Weighted


        #region Bottom-Weighted

        /// <summary>
        /// Returns a random float [0, 1).
        /// Lower values are more likely to be returned than higher ones.
        /// </summary>
        /// <returns>A bottom-weighted random float [0, 1).</returns>
        public static float FloatBottomWeighted()
        {
            const float MinInclusive = 0.0f;
            const float MaxExclusive = 1.0f;

            return FloatBottomWeighted(MinInclusive, MaxExclusive);
        }

        /// <summary>
        /// Returns a random float [0, <paramref name="maxExclusive"/>).
        /// Lower values are more likely to be returned than higher ones.
        /// </summary>
        /// <returns>A bottom-weighted random float [0, <paramref name="maxExclusive"/>).</returns>
        public static float FloatBottomWeighted(float maxExclusive)
        {
            const float MinInclusive = 0.0f;
            return FloatBottomWeighted(MinInclusive, maxExclusive);
        }

        /// <summary>
        /// Returns a random float [<paramref name="minInclusive"/>, <paramref name="maxExclusive"/>).
        /// Lower values are more likely to be returned than higher ones.
        /// </summary>
        /// <returns>A bottom-weighted random float [<paramref name="minInclusive"/>, <paramref name="maxExclusive"/>).</returns>
        public static float FloatBottomWeighted(float minInclusive, float maxExclusive)
        {
            float ratio = FloatTopWeighted(0.0f, 1.0f);
            ratio = 1.0f - ratio;

            float range = maxExclusive - minInclusive;
            float value = minInclusive + (ratio * range);
            return value;
        }

        #endregion Bottom-Weighted


        /// <summary>
        /// Returns a random float [0, 1.0f).
        /// The closer a value is to <paramref name="peak"/>,
        /// the more likely it is to be selected.
        /// (This trend is only approximately true when high levels of precision
        /// are needed in the results.)
        /// </summary>
        /// <param name="peak"></param>
        /// <returns>A peak-weighted float [0, <paramref name="maxExclusive"/>).</returns>
        public static float RatioAroundPeak(float peak)
        {
            const float MinInclusive = 0.0f;
            const float MaxExclusive = 1.0f;
#if UNITY_EDITOR
            Debug.Assert(MinInclusive <= peak && peak <= MaxExclusive);
#endif

            bool isLeftSide = RandomUtil.Bool(peak);

            float value = isLeftSide ? FloatTopWeighted(MinInclusive, peak)
                            : FloatBottomWeighted(peak, MaxExclusive);

#if UNITY_EDITOR
            Debug.Assert(MinInclusive <= value && value < MaxExclusive);
#endif

            return value;
        }

        #endregion Float


        #region Int

        #region Top-Weighted

        /// <summary>
        /// Returns a random int [0, <paramref name="maxExclusive"/>).
        /// Higher values are more likely to be returned than lower ones.
        /// </summary>
        /// <returns>A top-weighted random int [0, <paramref name="maxExclusive"/>).</returns>
        public static int IntTopWeighted(int maxExclusive)
        {
            int ret = IntTopWeighted(0, maxExclusive);
            return ret;
        }

        /// <summary>
        /// Returns a random int [<paramref name="minInclusive"/>,
        /// <paramref name="maxExclusive"/>).
        /// Higher values are more likely to be returned than lower ones.
        /// </summary>
        /// <returns>A top-weighted random int [<paramref name="minInclusive"/>,
        /// <paramref name="maxExclusive"/>).</returns>
        public static int IntTopWeighted(int minInclusive, int maxExclusive)
        {
            int ret = (int)(FloatTopWeighted(minInclusive, maxExclusive));
#if UNITY_EDITOR
            Debug.Assert(ret < maxExclusive);
#endif
            return ret;
        }

        #endregion Top-Weighted


        #region Bottom-Weighted

        /// <summary>
        /// Returns a random int [0, <paramref name="maxExclusive"/>).
        /// Lower values are more likely to be returned than higher ones.
        /// </summary>
        /// <returns>A top-weighted random int [0, <paramref name="maxExclusive"/>).</returns>
        public static int IntBottomWeighted(int maxExclusive)
        {
            int ret = IntBottomWeighted(0, maxExclusive);
            return ret;
        }

        /// <summary>
        /// Returns a random int [<paramref name="minInclusive"/>,
        /// <paramref name="maxExclusive"/>).
        /// Lower values are more likely to be returned than higher ones.
        /// </summary>
        /// <returns>A top-weighted random int [<paramref name="minInclusive"/>,
        /// <paramref name="maxExclusive"/>).</returns>
        public static int IntBottomWeighted(int minInclusive, int maxExclusive)
        {
            int ret = (int)(FloatBottomWeighted(minInclusive, maxExclusive));
#if UNITY_EDITOR
            Debug.Assert(ret < maxExclusive);
#endif
            return ret;
        }

        #endregion Bottom-Weighted


        /// <summary>
        /// Returns a random int [0, <paramref name="maxExclusive"/>).
        /// The closer a value is to (<paramref name="peakRatio"/> * <paramref name="maxExclusive"/>),
        /// the more likely it is to be selected.
        /// (This trend is only approximately true when high levels of precision
        /// are needed in the results.)
        /// </summary>
        /// <param name="maxExclusive">One greater than the highest possible integer to be selected.</param>
        /// <param name="peakRatio">The ratio [0, 1) of the most common integer to return compared to <paramref name="maxExclusive"/></param>
        /// <returns>A peak-weighted int [0, <paramref name="maxExclusive"/>).</returns>
        public static int IndexAroundPeakRatioUnadjusted(int maxExclusive, float peakRatio)
        {
            int ret = (int)(RatioAroundPeak(peakRatio) * maxExclusive);
            return ret;
        }

        /// <summary>
        /// Returns a random int [0, <paramref name="maxExclusive"/>).
        /// The closer a value is to (<paramref name="peakRatio"/> * <paramref name="maxExclusive"/>),
        /// the more likely it is to be selected.
        /// (This trend is only approximately true when high levels of precision
        /// are needed in the results.)
        ///
        /// <para>The unadjusted version of this method has a disproportionately low chance
        /// of returning the lowest or highest possible index. This version gives a
        /// small buffer of extra weight to each extreme.</para>
        /// </summary>
        /// <param name="maxExclusive">One greater than the highest possible integer to be selected.</param>
        /// <param name="peakRatio">The ratio [0, 1) of the most common integer to return compared to <paramref name="maxExclusive"/></param>
        /// <returns>A peak-weighted int [0, <paramref name="maxExclusive"/>).</returns>
        public static int IndexAroundPeakRatio(int maxExclusive, float peakRatio)
        {
            const float Delta = 0.4f;
            const float DeltaTwo = Delta * 2.0f;

            float index = RatioAroundPeak(peakRatio) * (maxExclusive + DeltaTwo);
            index -= Delta;

            float maxIndex = maxExclusive - 1;
            index = Mathf.Clamp(index, 0.0f, maxIndex);

            int ret = (int)index;
            return ret;
        }

        #endregion Int
    }
}
