using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public static class LinqUtil
    {
        /// <summary>
        /// Returns an array of a given size
        /// in which each value is equal to a given default value.
        /// </summary>
        /// <param name="sizeOfArray">The size of the array.</param>
        /// <param name="arrayValue">The value to set each value of the array.</param>
        /// <returns>The filled array.</returns>
        public static T[] UniformArray<T>(int sizeOfArray, T arrayValue)
        {
            T[] ret = Enumerable.Range(0, sizeOfArray)
                .Select(x => arrayValue).ToArray();
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
        public static T[] Array<T>(int sizeOfArray, Func<T> selector)
        {
            T[] ret = Enumerable.Range(0, sizeOfArray)
                .Select(x => selector()).ToArray();
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
        public static TResult[] Array<TResult>(int sizeOfArray, Func<int, TResult> selector)
        {
            TResult[] ret = Enumerable.Range(0, sizeOfArray)
                .Select(x => selector(x)).ToArray();
            return ret;
        }
    }
}
