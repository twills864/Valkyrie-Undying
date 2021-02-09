using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public static class ExceptionUtil
    {
        /// <summary>
        /// Throws a new ArgumentException based on the given argument.
        /// </summary>
        /// <typeparam name="T">The type of the excepted argument.</typeparam>
        /// <param name="argument">The excepted argument</param>
        /// <returns>A new ArgumentException to throw.</returns>
        public static ArgumentException ArgumentException<T>(T argument)
        {
            var ret = new ArgumentException($"Unknown {typeof(T)} {argument}");
            return ret;
        }
    }
}
