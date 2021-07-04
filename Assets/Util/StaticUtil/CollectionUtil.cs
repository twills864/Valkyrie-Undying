using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    /// <summary>
    /// Contains useful methods for manipulating C# collections.
    /// </summary>
    public static class CollectionUtil
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


        /// <summary>
        /// Gets the keyed value from a given Dictionary, or adds a new value
        /// if the given key is not present.
        /// </summary>
        /// <typeparam name="TKey">The type of the key to access from the Dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the value to retrieve from the Dictionary.</typeparam>
        /// <param name="source">The source Dictionary.</param>
        /// <param name="key">The key to use to access the Dictionary.</param>
        /// <returns>The value from <paramref name="source"/> accessible by <paramref name="key"/>.</returns>
        public static TValue GetOrAddNew<TKey, TValue>(IDictionary<TKey, TValue> source, TKey key) where TValue : new()
        {
            if (!source.TryGetValue(key, out TValue ret))
                ret = source[key] = new TValue();
            return ret;
        }

        /// <summary>
        /// Gets the keyed value from a given Dictionary, or adds a new default value
        /// if the given key is not present.
        /// </summary>
        /// <typeparam name="TKey">The type of the key to access from the Dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the value to retrieve from the Dictionary.</typeparam>
        /// <param name="source">The source Dictionary.</param>
        /// <param name="key">The key to use to access the Dictionary.</param>
        /// <param name="newValue">The function used to assign the new value if the given
        /// <paramref name="key"/> does not exist.</param>
        /// <returns>The value from <paramref name="source"/> accessible by <paramref name="key"/>.</returns>
        public static TValue GetOrAdd<TKey, TValue>(IDictionary<TKey, TValue> source, TKey key, Func<TValue> newValue)
        {
            if (!source.TryGetValue(key, out TValue ret))
                ret = source[key] = newValue();
            return ret;
        }
    }
}
