using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LogUtilAssets.Util.StaticUtil;

namespace Assets.Util
{
    /// <summary>
    /// Contains useful methods for generating descriptive exceptions to be used in code.
    /// </summary>
    public static class ExceptionUtil
    {
        /// <summary>
        /// Throws a new ArgumentException based on the given argument.
        /// </summary>
        /// <typeparam name="T">The type of the excepted argument.</typeparam>
        /// <param name="argument">The excepted argument.</param>
        /// <returns>A new ArgumentException to throw.</returns>
        public static ArgumentException ArgumentException<T>(T argument)
        {
            var ret = new ArgumentException($"Unknown {typeof(T).Name} {argument}");
            return ret;
        }

        /// <summary>
        /// Throws a new ArgumentException based on the given argument expression.
        /// </summary>
        /// <param name="expression">An expression representing the excepted argument.</param>
        /// <returns>A new ArgumentException to throw.</returns>
        public static ArgumentException ArgumentException(Expression<Func<object>> expression)
        {
            var memberInfo = LambdaUtil.GetMemberInfo(expression);
            var argumentName = memberInfo.Key;
            var argumentValue = memberInfo.Value;
            var argumentType = argumentValue.GetType();

            var ret = new ArgumentException($"Unknown {argumentType.Name} {argumentValue}", argumentName);
            return ret;
        }
    }
}
