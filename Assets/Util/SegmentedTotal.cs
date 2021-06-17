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
    /// </summary>
    public struct SegmentedTotal
    {
        #region Property Fields
        private int _total;
        private int _item1;
        private int _item2;
        private int _item3;
        private int _item4;
        #endregion Property Fields

        public int Total { get => _total; private set => _total = value; }

        public int Item1 { get => _item1; set => UpdateItem(ref _item1, value); }
        public int Item2 { get => _item2; set => UpdateItem(ref _item1, value); }
        public int Item3 { get => _item3; set => UpdateItem(ref _item1, value); }
        public int Item4 { get => _item4; set => UpdateItem(ref _item1, value); }

        private void UpdateItem(ref int item, int value)
        {
            Total += (value - item);
            item = value;
        }

        public void Reset()
        {
            _total = 0;
            _item1 = 0;
            _item2 = 0;
            _item3 = 0;
            _item4 = 0;
        }
    }
}
