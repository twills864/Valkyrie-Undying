using System;
using System.Reflection;

namespace Assets.Constants
{
    public static class ObsoleteConstants
    {
        //public const string SpeedOfDevelopment =
        //    "This method was created for ease and speed of development. " +
        //    "It should be replaced with a hard-coded version upon release.";

        public const string EngineOverhaul = "Deprecated as part of the Valkyrie engine overhaul.";
        public const string FollowTheFun = "This feature is fully functional, but isn't fun.";

        /// <summary>
        /// Returns true if the given type has the Obsolete attribute.
        /// </summary>
        /// <param name="type">The type to check for obsolescence.</param>
        /// <returns>True if the type is obsolete; false otherwise.</returns>
        public static bool IsObsolete(Type type)
        {
            var attribute = type.GetCustomAttribute<ObsoleteAttribute>(false);
            bool ret = attribute != null;
            return ret;
        }
    }
}