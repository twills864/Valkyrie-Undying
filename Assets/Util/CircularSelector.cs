using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    // Variation of a circular array that returns ones element at a time.
    // Mostly used for debugging
    public class CircularSelector<T> : List<T>
    {
        private int Index = 0;

        public T Current => this[Index];

        public void Increment()
        {
            Index++;
            Index %= Count;
        }
        public T GetAndIncrement()
        {
            T ret = Current;
            Increment();
            return ret;
        }
    }
}
