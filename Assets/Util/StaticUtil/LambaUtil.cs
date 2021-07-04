using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    /// <summary>
    /// Class adapted from source code provided by automatetheplanet.com
    /// https://www.automatetheplanet.com/get-property-names-using-lambda-expressions/
    /// </summary>
    internal static class LambdaUtil
    {
        /// <summary>
        /// Returns the name and value of the given expression.
        /// </summary>
        /// <param name="expression">The expression to examine.</param>
        /// <returns>A key value pair in which the key is the name of the given expression,
        /// and the value is the value of the given expression.</returns>
        public static KeyValuePair<string, object> GetMemberInfo(Expression<Func<object>> expression)
        {
            string key = GetMemberName(expression);

            var method = expression.Compile();
            object value = method();

            var ret = new KeyValuePair<string, object>(key, value);
            return ret;
        }

        /// <summary>
        /// Returns the name and value of a given expression.
        /// </summary>
        /// <param name="expression">The expression to examine.</param>
        /// <returns>A key value pair in which the key is the name of the given expression,
        /// and the value is the value of the given expression.</returns>
        public static KeyValuePair<string, object> GetMemberInfo<T>(T source, Expression<Func<T, object>> expression)
        {
            string key = GetMemberName(expression);

            var method = expression.Compile();
            object value = method(source);

            var ret = new KeyValuePair<string, object>(key, value);
            return ret;
        }

        /// <summary>
        /// Returns the name of a given expression.
        /// </summary>
        /// <param name="expression">The expression to examine.</param>
        /// <returns>The name of the given expression.</returns>
        public static string GetMemberName(Expression<Func<object>> expression)
        {
            return GetMemberName(expression.Body);
        }

        /// <summary>
        /// Returns the name of a given expression.
        /// </summary>
        /// <param name="expression">The expression to examine.</param>
        /// <returns>The name of the given expression.</returns>
        public static string GetMemberName<T>(Expression<Func<T, object>> expression)
        {
            return GetMemberName(expression.Body);
        }

        /// <summary>
        /// Returns the name of a given expression.
        /// </summary>
        /// <param name="expression">The expression to examine.</param>
        /// <returns>The name of the given expression.</returns>
        private static string GetMemberName(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentException("Expression cannot be null");
            }
            // Reference type property or field
            if (expression is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }
            // Reference type method
            if (expression is MethodCallExpression methodCallExpression)
            {
                return methodCallExpression.Method.Name;
            }
            // Property, field of method returning value type
            if (expression is UnaryExpression unaryExpression)
            {
                return GetMemberName(unaryExpression);
            }
            string invalidExpressionMessage = $"Invalid expression {expression}";
            throw new ArgumentException(invalidExpressionMessage);
        }

        /// <summary>
        /// Returns the name of a given UnaryExpression.
        /// </summary>
        /// <param name="unaryExpression">The UnaryExpression to examine.</param>
        /// <returns>The name of the given UnaryExpression.</returns>
        private static string GetMemberName(UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression methodExpression)
            {
                return methodExpression.Method.Name;
            }
            return ((MemberExpression)unaryExpression.Operand).Member.Name;
        }
    }
}
