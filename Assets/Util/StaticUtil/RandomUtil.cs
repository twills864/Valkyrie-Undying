using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public static class RandomUtil
    {
        private static System.Random Random { get; set; }

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

        #endregion Bool
    }
}
