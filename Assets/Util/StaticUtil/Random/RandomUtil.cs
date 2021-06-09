using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

using Random = System.Random;

namespace Assets.Util
{
    public static class RandomUtil
    {
        private static System.Random Random { get; } = new Random();

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
        /// Returns a random float [0, 1).
        /// </summary>
        /// <returns>A random float [0, 1).</returns>
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
        public static float Float(float minInclusive, float maxExclusive)
        {
            return (float)Double(minInclusive, maxExclusive);
        }

        /// <summary>
        /// Returns a random float [<paramref name="minInclusive"/>,
        /// <paramref name="maxExclusive"/>).
        /// Also provides the ratio [0, 1] used to generate the value.
        /// </summary>
        /// <returns>A random float [<paramref name="minInclusive"/>,
        /// <paramref name="maxExclusive"/>).</returns>
        public static float Float(float minInclusive, float maxExclusive, out float ratio)
        {
            float ret = (float)Double(minInclusive, maxExclusive, out double outRatio);
            ratio = (float)outRatio;
            return ret;
        }

        #endregion Float

        #region Double

        /// <summary>
        /// Returns a random double [0, 1).
        /// </summary>
        /// <returns>A random double [0, 1).</returns>
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

        /// <summary>
        /// Returns a random double [<paramref name="minInclusive"/>,
        /// <paramref name="maxExclusive"/>).
        /// Also provides the ratio [0, 1) used to generate the value.
        /// </summary>
        /// <returns>A random double [<paramref name="minInclusive"/>,
        /// <paramref name="maxExclusive"/>).</returns>
        public static double Double(double minInclusive, double maxExclusive, out double ratio)
        {
            double range = maxExclusive - minInclusive;
            ratio = Random.NextDouble();
            double ret = (ratio * range) + minInclusive;
            return ret;
        }

        #endregion Double

        #region Negative Values

        /// <summary>
        /// Returns a given number, or the negative of the given number, each with equal probability.
        /// </summary>
        /// <param name="input">The number to randomize.</param>
        /// <returns>The result of the randomization.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RandomlyNegative(int input)
        {
            int ret = Bool() ? input : -input;
            return ret;
        }

        /// <summary>
        /// Returns a given number, or the negative of the given number, each with equal probability.
        /// </summary>
        /// <param name="input">The number to randomize.</param>
        /// <returns>The result of the randomization.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RandomlyNegative(float input)
        {
            float ret = Bool() ? input : -input;
            return ret;
        }

        /// <summary>
        /// Returns a given number, or the negative of the given number, each with equal probability.
        /// </summary>
        /// <param name="input">The number to randomize.</param>
        /// <returns>The result of the randomization.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double RandomlyNegative(double input)
        {
            double ret = Bool() ? input : -input;
            return ret;
        }


        #endregion Negative Values

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
        public static bool BoolPercent(int percentChanceOfTrue)
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

        #region Vectors

        /// <summary>
        /// Returns a vector of a specified <paramref name="length"/> in a pseudo-random direction.
        /// </summary>
        /// <param name="length">The length of the vector to return.</param>
        /// <returns>The random-direction vector of the specified length.</returns>
        public static Vector3 RandomDirectionVector(float length)
        {
            var ret = length * RandomDirectionVector();
            return ret;
        }

        /// <summary>
        /// Returns a unit vector in a pseudo-random direction.
        /// </summary>
        /// <returns>The random-direction unit vector.</returns>
        public static Vector3 RandomDirectionVector()
        {
            const float VectorMax = 1;
            const float VectorHalf = VectorMax * 0.5f;

            float x = Float(VectorMax) - VectorHalf;
            float y = Float(VectorMax) - VectorHalf;

            Vector3 ret = MathUtil.DefaultVector3IfZero(new Vector3(x, y));
            ret.Normalize();

            return ret;
        }

        /// <summary>
        /// Returns a unit vector in a pseudo-random direction
        /// between diagonal-top-left and diagonal-top-right
        /// </summary>
        /// <returns>The random-direction unit vector.</returns>
        public static Vector3 RandomDirectionVectorTopQuarter()
        {
            float y = 1f;
            float x = Float(-y, y);

            Vector3 ret = new Vector3(x, y);
            ret.Normalize();
            return ret;
        }

        /// <summary>
        /// Returns a vector of a specified <paramref name="length"/> in a pseudo-random direction
        /// between diagonal-top-left and diagonal-top-right
        /// </summary>
        /// <returns>The random-direction vector.</returns>
        public static Vector3 RandomDirectionVectorTopQuarter(float length)
        {
            Vector3 ret = RandomDirectionVectorTopQuarter();
            ret *= length;

            return ret;
        }



        /// <summary>
        /// Returns a unit vector in a pseudo-random direction
        /// excluding the angles between diagonal-bottom-left and diagonal-bottom-right
        /// </summary>
        /// <returns>The random-direction unit vector.</returns>
        public static Vector3 RandomDirectionVectorTopThreeQuarters()
        {
            const float AngleBottomRight = -45f;
            const float AngleBottomLeft = 225f;

            float angleDegrees = Float(AngleBottomRight, AngleBottomLeft);
            Vector3 ret = MathUtil.Vector3AtDegreeAngle(angleDegrees);
            return ret;
        }

        /// <summary>
        /// Returns a vector of a specified <paramref name="length"/> in a pseudo-random direction
        /// excluding the angles between diagonal-bottom-left and diagonal-bottom-right
        /// </summary>
        /// <returns>The random-direction vector.</returns>
        public static Vector3 RandomDirectionVectorTopThreeQuarters(float length)
        {
            Vector3 ret = RandomDirectionVectorTopThreeQuarters();
            ret *= length;

            return ret;
        }

        private static Vector2 RandomPointInBounds(Collider2D collider, Vector2 min, Vector2 max)
        {
            Vector2 point;

#if UNITY_EDITOR
            const int MaxCount = 10000;
            int count = 0;
#endif

            do
            {
#if UNITY_EDITOR
                if (count >= MaxCount)
                {
                    Debug.LogError("ERROR: Random point inside collider not found after 10000 iterations.", collider);
                    Debug.LogError($"MIN: {min}", collider);
                    Debug.LogError($"MAX: {max}", collider);
                    Debug.LogError($"BOUNDS: {collider.bounds}", collider);
                    Debug.LogError($"\r\n\r\n\r\n\r\n\r\n{RandomUtil.Float()}", collider);

                    return (min + max) * 0.5f;
                }
                count++;
#endif

                point.x = Float(min.x, max.x);
                point.y = Float(min.y, max.y);
            } while (!collider.OverlapPoint(point));

            return point;
        }

        public static Vector2 RandomPointInsiderCollider(Collider2D collider)
        {
            Vector2 min = collider.bounds.min;
            Vector2 max = collider.bounds.max;

            Vector2 point = RandomPointInBounds(collider, min, max);

            return point;
        }

        public static Vector2[] RandomsPointsInsiderCollider(Collider2D collider, int count)
        {
            Vector2 min = collider.bounds.min;
            Vector2 max = collider.bounds.max;

            Vector2[] points = LinqUtil.Array(count, () => RandomPointInBounds(collider, min, max));
            return points;
        }

#endregion Vectors

#region Collections

        /// <summary>
        /// Returns a random element of a given IList&lt;<typeparamref name="T"/>&gt; with equal probabilities.
        /// </summary>
        /// <typeparam name="T">The type of the IList&lt;<typeparamref name="T"/>&gt;.</typeparam>
        /// <param name="source">The IList&lt;<typeparamref name="T"/>&gt;.</param>
        /// <returns>A random element from the IList&lt;<typeparamref name="T"/>&gt;.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T RandomElement<T>(IList<T> source)
        {
            int index = Int(source.Count);
            T ret = source[index];
            return ret;
        }

        /// <summary>
        /// Attempts to retrieve a random element of a given IList&lt;<typeparamref name="T"/>&gt;.
        /// Returns false if no element could be retrieved because the given <paramref name="source"/> was empty.
        /// Returns true otherwise.
        /// </summary>
        /// <typeparam name="T">The type of the IList&lt;<typeparamref name="T"/>&gt;.</typeparam>
        /// <param name="source">The IList&lt;<typeparamref name="T"/>&gt; to retrieve a random element from.</param>
        /// <param name="randomElement">The random element retrieved from the <paramref name="source"/>.</param>
        /// <returns>True if a random element was retrieved; false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetRandomElement<T>(IList<T> source, out T randomElement)
        {
            if(!source.Any())
            {
                randomElement = default;
                return false;
            }
            else
            {
                randomElement = RandomElement(source);
                return true;
            }
        }

        /// <summary>
        /// Returns a random element of a given IEnumerable&lt;<typeparamref name="T"/>&gt; with equal probabilities.
        /// </summary>
        /// <typeparam name="T">The type of the IEnumerable&lt;<typeparamref name="T"/>&gt;.</typeparam>
        /// <param name="source">The IEnumerable&lt;<typeparamref name="T"/>&gt;.</param>
        /// <returns>A random element from the IEnumerable&lt;<typeparamref name="T"/>&gt;.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T RandomElement<T>(IEnumerable<T> source)
        {
            int count = source.Count();
            int randomIindex = Int(count);

            T ret = source.Skip(randomIindex).First();
            return ret;
        }

        /// <summary>
        /// Attempts to retrieve a random element of a given IEnumerable&lt;<typeparamref name="T"/>&gt;.
        /// Returns false if no element could be retrieved because the given <paramref name="source"/> was empty.
        /// Returns true otherwise.
        /// </summary>
        /// <typeparam name="T">The type of the IEnumerable&lt;<typeparamref name="T"/>&gt;.</typeparam>
        /// <param name="source">The IEnumerable&lt;<typeparamref name="T"/>&gt; to retrieve a random element from.</param>
        /// <param name="randomElement">The random element retrieved from the <paramref name="source"/>.</param>
        /// <returns>True if a random element was retrieved; false otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetRandomElement<T>(IEnumerable<T> source, out T randomElement)
        {
            if (!source.Any())
            {
                randomElement = default;
                return false;
            }
            else
            {
                randomElement = RandomElement(source);
                return true;
            }
        }

        /// <summary>
        /// Rearranges a given IList&lt;<typeparamref name="T"/>&gt; to a completely random order.
        /// Based on the Fisher-Yates shuffle algorithm.
        /// </summary>
        /// <typeparam name="T">The type of the IList&lt;<typeparamref name="T"/>&gt;.</typeparam>
        /// <param name="source">The array to be shuffled.</param>
        /// <see cref="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle">Fisher-Yates shuffle algorithm.</see>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Shuffle<T>(IList<T> source)
        {
            int i = source.Count;
            while (i > 1)
            {
                int j = Int(i--);
                CollectionUtil.Swap(source, i, j);
            }
        }

        /// <summary>
        /// Creates a copy of a given IList&lt;<typeparamref name="T"/>&gt;, and
        /// shuffles it to a completely random order.
        /// Based on the Fisher-Yates shuffle algorithm.
        /// </summary>
        /// <typeparam name="T">The type of the IList&lt;<typeparamref name="T"/>&gt;.</typeparam>
        /// <param name="source">The array to be shuffled.</param>
        /// <returns>A new array with the same elements as source, but in a random order.</returns>
        /// <see cref="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle">Fisher-Yates shuffle algorithm.</see>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ShuffleNew<T>(IList<T> source)
        {
            var ret = new T[source.Count];
            Array.Copy(source.ToArray(), ret, source.Count);
            Shuffle(ret);
            return ret;
        }

        /// <summary>
        /// Attempts to retrieve up to a specified number of random elements from a given IList&lt;<typeparamref name="T"/>&gt;.
        /// May retrieve fewer than the specified number if the source has fewer elements than requested.
        /// </summary>
        /// <typeparam name="T">The type of the IList to get random elements from.</typeparam>
        /// <param name="source">The IList&lt;<typeparamref name="T"/>&gt; to retrieve random elements from.</param>
        /// <param name="maxNumToGet">The upper limit of elements to get.</param>
        /// <returns></returns>
        public static T[] GetUpToXRandomElements<T>(IList<T> source, int maxNumToGet)
        {
            T[] ret;
            if (!source.Any())
                ret = new T[0];
            else if (source.Count <= maxNumToGet)
                ret = source.ToArray();
            else
            {
                var sourceArray = source.ToArray();
                var tempArray = new T[sourceArray.Length];
                Array.Copy(sourceArray, tempArray, sourceArray.Length);

                Shuffle(tempArray);

                ret = new T[maxNumToGet];
                Array.Copy(tempArray, ret, maxNumToGet);
            }

            return ret;
        }

#endregion Collections

#region Select

        /// <summary>
        /// Returns a random element from the specified parameters with equal probability.
        /// </summary>
        /// <typeparam name="T">The type of element to return.</typeparam>
        /// <param name="one">The first possible option.</param>
        /// <param name="two">The second possible option.</param>
        /// <returns>A random element from the specified parameters.</returns>
        public static T Select<T>(T one, T two)
        {
            T ret = Bool() ? one : two;
            return ret;
        }

#endregion Select
    }
}
