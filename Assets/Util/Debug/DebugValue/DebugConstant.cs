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
