using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public static class RandomUtil
    {
        private static System.Random Random { get; set; }
            = new Random();

        public static void Init()
        {
            Random = new Random();
        }
        public static void Init(int seed)
        {
            Random = new Random(seed);
        }

        #region Int

        /// <summary>
        /// Returns a random int.
        /// </summary>
        /// <returns>A random int.</returns>
        public static int Int()
        {
            int ret = Random.Next();
            return ret;
        }

        /// <summary>
        /// Returns a random int [0, <paramref name="maxExclusive"/>).
        /// </summary>
        /// <returns>A random int [0, <paramref name="maxExclusive"/>).</returns>
        public static int Int(int maxExclusive)
        {
            int ret = Random.Next(maxExclusive);
            return ret;
        }

        /// <summary>
        /// Returns a random int [<paramref name="minInclusive"/>,
        /// <paramref name="maxExclusive"/>).
        /// </summary>
        /// <returns>A random int [<paramref name="minInclusive"/>,
        /// <paramref name="maxExclusive"/>).</returns>
        public static int Int(int minInclusive, int maxExclusive)
        {
            int ret = Random.Next(minInclusive, maxExclusive);
            return ret;
        }

        #endregion Int

        #region Float

        /// <summary>
        /// Returns a random float.
        /// </summary>
        /// <returns>A random float.</returns>
        public static float Float()
        {
            return (float) Double();
        }

        /// <summary>
        /// Returns a random float [0.0f, <paramref name="maxExclusive"/>).
        /// </summary>
        /// <returns>A random float [0.0f, <paramref name="maxExclusive"/>).</returns>
        public static float Float(float maxExclusive)
        {
            return (float)Double(maxExclusive);
        }

        /// <summary>
        /// Returns a random float [<paramref name="minInclusive"/>,
        /// <paramref name="maxExclusive"/>).
        /// </summary>
        /// <returns>A random float [<paramref name="minInclusive"/>,
        /// <paramref name="maxExclusive"/>).</returns>
        public static float Float(double minInclusive, double maxExclusive)
        {
            return (float)Double(minInclusive, maxExclusive);
        }

        #endregion Float

        #region Double

        /// <summary>
        /// Returns a random double.
        /// </summary>
        /// <returns>A random double.</returns>
        public static double Double()
        {
            double ret = Random.NextDouble();
            return ret;
        }

        /// <summary>
        /// Returns a random double [0.0, <paramref name="maxExclusive"/>).
        /// </summary>
        /// <returns>A random double [0.0, <paramref name="maxExclusive"/>).</returns>
        public static double Double(double maxExclusive)
        {
            double ret = Random.NextDouble() * maxExclusive;
            return ret;
        }

        /// <summary>
        /// Returns a random double [<paramref name="minInclusive"/>,
        /// <paramref name="maxExclusive"/>).
        /// </summary>
        /// <returns>A random double [<paramref name="minInclusive"/>,
        /// <paramref name="maxExclusive"/>).</returns>
        public static double Double(double minInclusive, double maxExclusive)
        {
            double range = maxExclusive - minInclusive;
            double ret = (Random.NextDouble() * range) + minInclusive;
            return ret;
        }

        #endregion Double

        #region Bool

        /// <summary>
        /// Returns a random bool with an equal chance of returning true or false.
        /// </summary>
        public static bool Bool()
        {
            return Bool(0.5);
        }

        /// <summary>
        /// Randomly returns true with odds represented by <paramref name="chanceOfTrue"/>.
        /// </summary>
        /// <param name="chanceOfTrue">The chance of returning true, [0.0, 1.0]</param>
        public static bool Bool(double chanceOfTrue)
        {
            double check = Double();
            bool ret = check < chanceOfTrue;
            return ret;
        }

        /// <summary>
        /// Randomly returns true <paramref name="percentChanceOfTrue"/>% of the time.
        /// </summary>
        /// <param name="chanceOfTrue">The chance of returning true, [0, 100]</param>
        public static bool Bool(int percentChanceOfTrue)
        {
            int check = Int(100);
            bool ret = check < percentChanceOfTrue;
            return ret;
        }

        /// <summary>
        /// Returns a given number of random bool,
        /// each with an equal chance of returning true or false.
        /// </summary>
        /// <param name="numToGet">The number of bools to get.</param>
        public static bool[] Bools(int numToGet)
        {
            var ret = LinqUtil.Array(numToGet, () => Bool());
            return ret;
        }

        /// <summary>
        /// Returns a given number of true bools
        /// randomly shuffled throughout an array of the requested size.
        /// </summary>
        /// <param name="sizeOfArray">The size of the array.</param>
        /// <param name="numTrue">The number of true bools within the array.</param>
        /// <returns>The given number of true bools shuffled throughout the array.</returns>
        public static bool[] ShuffledBools(int sizeOfArray, int numTrue)
        {
#if DEBUG
            if (numTrue > sizeOfArray)
            {
                var message = "numTrue must not be greater than sizeOfArray " +
                    $"({numTrue} > {sizeOfArray}). " +
                    "(Did you reverse the order of the parameters?)";
                throw new ArgumentException(message, "numTrue");
            }
#endif
            bool[] ret = new bool[sizeOfArray];
            for (int i = 0; i < numTrue; i++)
                ret[i] = true;

            Shuffle(ret);
            return ret;
        }

        #endregion Bool

        #region Collections

        /// <summary>
        /// Returns a random element of a given IList&lt;<typeparamref name="T"/>&gt; with equal probabilities.
        /// </summary>
        /// <typeparam name="T">The type of the IList&lt;<typeparamref name="T"/>&gt;.</typeparam>
        /// <param name="source">The IList&lt;<typeparamref name="T"/>&gt;.</param>
        /// <returns>A random element from the IList&lt;<typeparamref name="T"/>&gt;.</returns>
        public static T RandomElement<T>(IList<T> source)
        {
            int index = Int(source.Count);
            T ret = source[index];
            return ret;
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
        private static void Swap<T>(IList<T> source, int index1, int index2)
        {
            T value = source[index1];
            source[index1] = source[index2];
            source[index2] = value;
        }

        /// <summary>
        /// Rearranges a given IList&lt;<typeparamref name="T"/>&gt; to a completely random order.
        /// Based on the Fisher-Yates shuffle algorithm.
        /// </summary>
        /// <typeparam name="T">The type of the IList&lt;<typeparamref name="T"/>&gt;.</typeparam>
        /// <param name="source">The array to be shuffled</param>
        /// <see cref="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle">Fisher-Yates shuffle algorithm.</see>
        public static void Shuffle<T>(IList<T> source)
        {
            int i = source.Count;
            while (i > 1)
            {
                int j = Int(i--);
                Swap(source, i, j);
            }
        }

        #endregion Collections
    }
}
