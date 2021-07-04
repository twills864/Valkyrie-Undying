namespace Assets.Util.AssetsDebug
{
    /// <summary>
    /// Represents a value to be displayed for debugging purposes.
    /// </summary>
    public abstract class DebugValue
    {
        protected abstract object InnerValue { get; }

        public string Value => InnerValue?.ToString() ?? "<NULL>";
    }
}
