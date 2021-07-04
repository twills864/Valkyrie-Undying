using System;
using System.Reflection;

namespace Assets.Constants
{
    /// <summary>
    /// Contains constants explaining why a given feature is obsolete.
    /// </summary>
    public static class ObsoleteConstants
    {
        //public const string SpeedOfDevelopment =
        //    "This method was created for ease and speed of development. " +
        //    "It should be replaced with a hard-coded version upon release.";

        public const string EngineOverhaul = "Deprecated as part of the Valkyrie engine overhaul.";
        public const string FollowTheFun = "This feature is fully functional, but isn't fun.";
        public const string NotProductionReady = "This feature isn't yet ready for production.";
        public const string CurrentlyUnused = "This feature is presumed to by fully functional, but isn't current used.";

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