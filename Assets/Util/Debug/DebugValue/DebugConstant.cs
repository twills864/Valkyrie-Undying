using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util.AssetsDebug
{
    public class DebugConstant : DebugValue
    {
        private object _Value;

        protected override object InnerValue => _Value;

        public DebugConstant(object value)
        {
            _Value = value;
        }
    }
}
