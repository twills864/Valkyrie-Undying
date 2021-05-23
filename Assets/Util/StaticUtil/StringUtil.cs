using System;
using System.Text;

namespace Assets.Util
{
    public static class StringUtil
    {
        #region Substrings

        #region Numerical

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

        #endregion Numerical

        /// <summary>
        /// Returns a substring of the <paramref name="source"/>
        /// starting one character after the first occurrence of
        /// <paramref name="substring"/>, and extending until the end of the string.
        /// </summary>
        /// <param name="source">The source string to parse.</param>
        /// <param name="substring">The substring to find within the source.</param>
        /// <returns>The substring represented of the source after the given substring.</returns>
        public static string TextAfterFirst(string source, string substring)
        {
            int startIndexInclusive = source.IndexOf(substring);

            if (startIndexInclusive == -1)
                throw new ArgumentException($"Source ({source}) must contain substring ({substring}).", "substring");

            startIndexInclusive += substring.Length;
            string ret = source.Substring(startIndexInclusive);
            return ret;
        }

        #endregion Substrings

        /// <summary>
        /// Adds a space before each capital letter after the first in a given input.
        /// Based on a solution provided by StackOverflow user Binary Worrier.
        /// https://stackoverflow.com/a/272929
        /// </summary>
        /// <param name="input">The string for which to add spaces.</param>
        /// <param name="preserveAcronyms">Whether or not to preserve acronyms, such as "IDE."</param>
        /// <returns>The input string with the requested spaces.</returns>
        public static string AddSpacesBeforeCapitals(string input, bool preserveAcronyms = true)
        {
            if (String.IsNullOrWhiteSpace(input))
                return String.Empty;

            StringBuilder newText = new StringBuilder(input.Length * 2);
            newText.Append(input[0]);

            for (int i = 1; i < input.Length; i++)
            {
                if (Char.IsUpper(input[i]))
                {
                    bool PreviousLetterIsLowercase()
                        => input[i - 1] != ' ' && !Char.IsUpper(input[i - 1]);

                    // Acronym end is detected when the previous character is also uppercase,
                    // and the next letter is not uppercase.
                    bool AcronymEndDetected()
                        => preserveAcronyms && Char.IsUpper(input[i - 1])
                            && i < input.Length - 1 && !Char.IsUpper(input[i + 1]);

                    if (PreviousLetterIsLowercase() || AcronymEndDetected())
                        newText.Append(' ');
                }
                newText.Append(input[i]);
            }

            return newText.ToString();
        }
    }
}
