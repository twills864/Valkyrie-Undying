using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    /// <summary>
    /// Creates a sequence of unique ID's starting at a specified start index.
    /// </summary>
    [DebuggerDisplay("{Start} -> {Next}")]
    public class UniqueIdGenerator
    {
        private int Start;
        private int Next;

        public UniqueIdGenerator(int start = 0)
        {
            Start = start;
            Next = start;
        }

        /// <summary>
        /// Returns the next unique ID in the sequence.
        /// </summary>
        /// <returns>The next unique ID in the sequence.</returns>
        public int GetNext()
        {
            int ret = Next++;
            return ret;
        }

        /// <summary>
        /// Returns the total length of the sequence thus far.
        /// </summary>
        /// <returns>The total length of the sequence thus far.</returns>
        public int Length()
        {
            int ret = Next - Start;
            return ret;
        }

        public static implicit operator int(UniqueIdGenerator generator)
        {
            int ret = generator.GetNext();
            return ret;
        }
    }
}
