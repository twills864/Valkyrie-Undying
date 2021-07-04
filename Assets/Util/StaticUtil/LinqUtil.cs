using System;
using System.Runtime.CompilerServices;

namespace Assets.Util
{
    /// <summary>
    /// Contains useful methods that generate or otherwise manipulate collections.
    /// </summary>
    public static class LinqUtil
    {
        /// <summary>
        /// Returns an array of a given size
        /// in which each value is equal to a given default value.
        /// </summary>
        /// <param name="sizeOfArray">The size of the array.</param>
        /// <param name="arrayValue">The value to set each value of the array.</param>
        /// <returns>The filled array.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] UniformArray<T>(int sizeOfArray, T arrayValue)
        {
            T[] ret = new T[sizeOfArray];
            for (int i = 0; i < sizeOfArray; i++)
                ret[i] = arrayValue;

            return ret;
        }

        /// <summary>
        /// Returns an array of a given size
        /// in which each value is set to the value of a given default function.
        /// The array is guaranteed to be uniform only if the selector is deterministic.
        /// </summary>
        /// <param name="sizeOfArray">The size of the array.</param>
        /// <param name="arrayValue">The function to evaluate for each value of the array.</param>
        /// <returns>The filled array.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] Array<T>(int sizeOfArray, Func<T> selector)
        {
            T[] ret = new T[sizeOfArray];
            for (int i = 0; i < sizeOfArray; i++)
                ret[i] = selector();

            return ret;
        }

        /// <summary>
        /// Returns an array of a given size
        /// in which each value is set to the value of a given default function.
        /// The array is guaranteed to be uniform only if the selector is deterministic.
        /// </summary>
        /// <param name="sizeOfArray">The size of the array.</param>
        /// <param name="arrayValue">The function to evaluate for each value of the array.</param>
        /// <returns>The filled array.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult[] Array<TResult>(int sizeOfArray, Func<int, TResult> selector)
        {
            TResult[] ret = new TResult[sizeOfArray];
            for (int i = 0; i < sizeOfArray; i++)
                ret[i] = selector(i);

            return ret;
        }
    }
}
