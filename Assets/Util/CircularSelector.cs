using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    // Variation of a circular array that returns ones element at a time.
    // Mostly used for debugging
    public class CircularSelector<T>
    {
        private List<T> List { get; set; }
        private int Index = 0;

        public T Current => List[Index];
        public void Increment()
        {
            Index++;
            Index %= List.Count;
        }

        public CircularSelector()
        {
            List = new List<T>();
        }
        public CircularSelector(IEnumerable<T> items)
        {
            List = items.ToList();
        }
        public CircularSelector(List<T> list)
        {
            List = list;
        }
    }
}
