using System;

namespace Assets.Util.AssetsDebug
{
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
