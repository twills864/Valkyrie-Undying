using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Assets
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class FinalDebugViewLevelAttribute : Attribute
    {
        public static bool IsFinal(Type type)
        {
            var attribute = type.GetCustomAttribute<FinalDebugViewLevelAttribute>(false);
            bool ret = attribute != null;
            return ret;
        }
    }
}
