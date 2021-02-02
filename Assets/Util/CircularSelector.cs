using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    /// <summary>
    /// A subclass of List&lt;<typeparamref name="T"/>&gt; that represents
    /// a variation of a circular array that returns one element at a time.
    /// </summary>
    /// <typeparam name="T">The type of object represented in this Circular Selector.</typeparam>
    public class CircularSelector<T> : List<T>
    {
        private int _index;
        public int Index {
            get => _index;
            set
            {
                if (value == Count)
                    _index = 0;
                else if (value > Count)
                    _index = value % Count;
                else if (value < 0)
                    _index = MathUtil.Mod(value, Count);
                else
                    _index = value;
            }
        }

        /// <summary>
        /// The current element being represented.
        /// </summary>
        public T Current => this[Index];

        /// <summary>
        /// Increments next current element to be represented.
        /// </summary>
        public void Increment()
        {
            _index++;
            if (_index == Count)
                _index = 0;
        }

        /// <summary>
        /// Decrements the next current element to be represented.
        /// </summary>
        public void Decrement()
        {
            if (_index > 0)
                _index--;
            else
                _index = Count - 1;
        }

        /// <summary>
        /// Gets the element currently being represented, and
        /// increments to the next element to be represented.
        /// </summary>
        /// <returns></returns>
        public T GetAndIncrement()
        {
            T ret = Current;
            Increment();
            return ret;
        }
    }
}
