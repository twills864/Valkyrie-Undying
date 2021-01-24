using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public static class RandomUtil
    {
        public static RandomFactory Tas { get; set; }
        public static RandomFactory General { get; set; }

        public static void Init()
        {
            General = new RandomFactory();
            Tas = new RandomFactory(General.Int());
        }
    }

    public class RandomFactory
    {
        private System.Random Random { get; set; }

        public RandomFactory() : this(DateTime.Now.Ticks.GetHashCode())
        {
        }
        public RandomFactory(int seed)
        {
            Random = new Random(seed);
        }

        #region Int

        public int Int()
        {
            int ret = Random.Next();
            return ret;
        }

        public int Int(int maxExclusive)
        {
            int ret = Random.Next(maxExclusive);
            return ret;
        }

        public int Int(int minInclusive, int maxExclusive)
        {
            int ret = Random.Next(minInclusive, maxExclusive);
            return ret;
        }

        #endregion Int

        #region Float

        public float Float()
        {
            return (float) Double();
        }

        public float Float(float maxExclusive)
        {
            return (float)Double(maxExclusive);
        }

        public float Float(double minInclusive, double maxExclusive)
        {
            return (float)Double(minInclusive, maxExclusive);
        }

        #endregion Float

        #region Double

        public double Double()
        {
            double ret = Random.NextDouble();
            return ret;
        }

        public double Double(double maxExclusive)
        {
            double ret = Random.NextDouble() * maxExclusive;
            return ret;
        }

        public double Double(double minInclusive, double maxExclusive)
        {
            double range = maxExclusive - minInclusive;
            double ret = (Random.NextDouble() * range) + minInclusive;
            return ret;
        }

        #endregion Double

        #region Bool

        public bool Bool()
        {
            return Bool(0.5);
        }

        public bool Bool(double chanceOfTrue)
        {
            double check = Double();
            bool ret = check < chanceOfTrue;
            return ret;
        }

        public bool Bool(int percentChanceOfTrue)
        {
            int check = Int(100);
            bool ret = check < percentChanceOfTrue;
            return ret;
        }

        #endregion Bool
    }
}
