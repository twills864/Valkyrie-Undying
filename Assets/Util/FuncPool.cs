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

        public FuncPool(List<Func<T>> list) : base(list)
        {
        }

        public static implicit operator FuncPool<T>(Func<T>[] funcs)
        {
            return new FuncPool<T>(funcs.ToList());
        }
    }
}
