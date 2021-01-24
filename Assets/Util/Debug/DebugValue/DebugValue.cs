using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util.AssetsDebug
{
    public abstract class DebugValue
    {
        protected abstract object InnerValue { get; }

        public string Value => InnerValue?.ToString() ?? "<NULL>";
    }
}
