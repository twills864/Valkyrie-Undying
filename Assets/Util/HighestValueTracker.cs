using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    /// <summary>
    /// Stores a value, and remembers the historical highest value ever assigned.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HighestValueTracker<T> where T : IComparable
    {
        private T _value;
        public T Value {
            get => _value;
            set {
                _value = value;

                bool isNewValueHigher = _value.CompareTo(HighestValue) > 0;
                if (isNewValueHigher)
                    HighestValue = _value;
            }
        }

        public T HighestValue { get; set; }

        public HighestValueTracker()
        {
        }
        public HighestValueTracker(T value)
        {
            Value = value;
            HighestValue = value;
        }

        public override string ToString()
        {
            return $"{Value}";
        }

        public static implicit operator T(HighestValueTracker<T> value) => value.Value;
    }
}
