using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public static class StringUtil
    {
        public static string TextBetween(string source, int startIndexExclusive, int endIndexExclusive)
        {
            string ret = source.Substring(startIndexExclusive + 1, endIndexExclusive - startIndexExclusive - 1);
            return ret;
        }
        public static string TextAfter(string source, int startIndexExclusive)
        {
            string ret = source.Substring(startIndexExclusive + 1);
            return ret;
        }
        public static string TextBefore(string source, int endIndexExclusive)
        {
            string ret = source.Substring(0, endIndexExclusive);
            return ret;
        }
    }
}
