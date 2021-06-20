using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    /// <summary>
    /// A struct designed to be used by other structs to keep track of a
    /// running total from multiple, clearly-defined sources.
    ///
    /// Equivalent to SegmentedTotal, but stores float values.
    /// </summary>
    public struct SegmentedTotalF
    {
        #region Property Fields
        private float _total;
        private float _item1;
        private float _item2;
        private float _item3;
        private float _item4;
        #endregion Property Fields

        public float Total { get => _total; private set => _total = value; }

        public float Item1 { get => _item1; set => UpdateItem(ref _item1, value); }
        public float Item2 { get => _item2; set => UpdateItem(ref _item2, value); }
        public float Item3 { get => _item3; set => UpdateItem(ref _item3, value); }
        public float Item4 { get => _item4; set => UpdateItem(ref _item4, value); }

        private void UpdateItem(ref float item, float value)
        {
            Total += (value - item);
            item = value;
        }

        public void Reset()
        {
            _total = 0f;
            _item1 = 0f;
            _item2 = 0f;
            _item3 = 0f;
            _item4 = 0f;
        }
    }
}
