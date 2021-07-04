using System;
using System.Linq;

namespace Assets.Util.AssetsDebug
{
    /// <summary>
    /// Evaluates multiple Funcs for debugging purposes.
    /// </summary>
    /// <inheritdoc />
    public class DebugMulti : DebugValue
    {
        private Func<Object>[] _Funcs;

        protected override object InnerValue
        {
            get
            {
                var enumerable = _Funcs.Select(x => $"{{{x()}}}");
                string ret = string.Join(" ", enumerable);
                return ret;
            }
        }

        public DebugMulti(params Func<Object>[] funcs)
        {
            _Funcs = funcs;
        }
    }
}
