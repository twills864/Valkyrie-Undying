using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    // Mostly used for debugging
    public class FuncPool<T> : CircularSelector<Func<T>>
    {
        public T Run()
        {
            T ret = Current();
            return ret;
        }
    }
}
