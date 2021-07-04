using System;

namespace Assets.Util.AssetsDebug
{
    /// <summary>
    /// Evaluates a given Func to be displayed for debugging purposes.
    /// </summary>
    /// <inheritdoc />
    public class DebugFunc : DebugValue
    {
        private Func<Object> _Func;

        protected override object InnerValue => _Func();

        public DebugFunc(Func<Object> func)
        {
            _Func = func;
        }
    }
}
