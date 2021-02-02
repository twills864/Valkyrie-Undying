using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    /// <summary>
    /// A subclass of CircularSelector&lt;Func&lt;<typeparamref name="T"/>&gt;&gt;
    /// that allows the user to run the element currently being represented.
    /// </summary>
    public class FuncPool<T> : CircularSelector<Func<T>>
    {
        public T Run()
        {
            T ret = Current();
            return ret;
        }
    }
}
