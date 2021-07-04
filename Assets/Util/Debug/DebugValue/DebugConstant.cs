namespace Assets.Util.AssetsDebug
{
    /// <summary>
    /// Displays a constant value for debugging purposes.
    /// </summary>
    /// <inheritdoc />
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
