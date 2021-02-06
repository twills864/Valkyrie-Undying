namespace Assets.Util
{
    public static class StringUtil
    {
        /// <summary>
        /// Returns a substring of the <paramref name="source"/>
        /// starting one character after <paramref name="startIndexExclusive"/>,
        /// and ending one character before <paramref name="endIndexExclusive"/>
        /// </summary>
        /// <param name="source">The source string to parse.</param>
        /// <param name="startIndexExclusive">The index before the start of the substring.</param>
        /// <param name="endIndexExclusive">The index after the end of the substring.</param>
        /// <returns>The substring represented by the given indices.</returns>
        public static string TextBetween(string source, int startIndexExclusive, int endIndexExclusive)
        {
            string ret = source.Substring(startIndexExclusive + 1, endIndexExclusive - startIndexExclusive - 1);
            return ret;
        }

        /// <summary>
        /// Returns a substring of the <paramref name="source"/>
        /// starting one character after <paramref name="startIndexExclusive"/>,
        /// and extending until the end of the string.
        /// </summary>
        /// <param name="source">The source string to parse.</param>
        /// <param name="startIndexExclusive">The index before the start of the substring.</param>
        /// <returns>The substring represented by the given index.</returns>
        public static string TextAfter(string source, int startIndexExclusive)
        {
            string ret = source.Substring(startIndexExclusive + 1);
            return ret;
        }

        /// <summary>
        /// Returns a substring of the <paramref name="source"/>
        /// starting at the beginning of the string,
        /// and ending one character before <paramref name="startIndexExclusive"/>.
        /// </summary>
        /// <param name="source">The source string to parse.</param>
        /// <param name="endIndexExclusive">The index before the start of the substring.</param>
        /// <returns>The substring represented by the given index.</returns>
        public static string TextBefore(string source, int endIndexExclusive)
        {
            string ret = source.Substring(0, endIndexExclusive);
            return ret;
        }
    }
}
