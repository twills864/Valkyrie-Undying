using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    /// <summary>
    /// Represents a string object using member variables that are parsed and assigned
    /// when the source string is assigned.
    /// </summary>
    public abstract class ParsedText
    {
        private string _source;

        protected string Source
        {
            get => _source;
            set
            {
                _source = value;
                ParseSource();
            }
        }

        /// <summary>
        /// <para>The method automatically called each time the source string is set.</para>
        /// <para>Used to parse the source string into respective member properties.</para>
        /// </summary>
        protected abstract void ParseSource();

        private ParsedText() { }

        public ParsedText(string source)
        {
            Source = source;
        }

        // Utility methods for parsing the source
        protected string TextBetween(int startIndexExclusive, int endIndexExclusive)
        {
            string ret = StringUtil.TextBetween(Source, startIndexExclusive, endIndexExclusive);
            return ret;
        }
        protected string TextAfter(int startIndexExclusive)
        {
            string ret = StringUtil.TextAfter(Source, startIndexExclusive);
            return ret;
        }
        protected string TextBefore(int endIndexExclusive)
        {
            string ret = StringUtil.TextBefore(Source, endIndexExclusive);
            return ret;
        }

        public override string ToString()
        {
            return Source;
        }
    }
}
