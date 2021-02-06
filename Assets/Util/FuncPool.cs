using System;

namespace Assets.Util
{
    /// <summary>
    /// A subclass of CircularSelector&lt;Func&lt;<typeparamref name="T"/>&gt;&gt;
    /// that allows the user to run the element currently being represented.
    /// </summary>
    /// <inheritdoc/>
    public class FuncPool<T> : CircularSelector<Func<T>>
    {
        public T Run()
        {
            T ret = Current();
            return ret;
        }
    }
}
