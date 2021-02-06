namespace Assets.Util.AssetsDebug
{
    public abstract class DebugValue
    {
        protected abstract object InnerValue { get; }

        public string Value => InnerValue?.ToString() ?? "<NULL>";
    }
}
