using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public static class CodeUtil
    {
        /// <summary>
        /// Swaps the values of any two provided elements.
        /// </summary>
        /// <typeparam name="T">The type elements to swap.
        /// <param name="one">The first element to swap.</param>
        /// <param name="two">The second element to swap.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(ref T one, ref T two)
        {
            T temp = one;
            one = two;
            two = temp;
        }

        /// <summary>
        /// Swaps the elements within a given source IList&lt;<typeparamref name="T"/>&gt;
        /// located at the given indices.
        /// </summary>
        /// <typeparam name="T">The type of the source IList&lt;<typeparamref name="T"/>&gt;</typeparam>.
        /// <param name="source">The source IList&lt;<typeparamref name="T"/>&gt;.</param>
        /// <param name="index1">The first index to swap.</param>
        /// <param name="index2">The second index to swap.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(IList<T> source, int index1, int index2)
        {
            T value = source[index1];
            source[index1] = source[index2];
            source[index2] = value;
        }
    }
}
