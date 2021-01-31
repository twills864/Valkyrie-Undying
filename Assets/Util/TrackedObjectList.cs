using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public class TrackedObjectList<T> : List<T> where T : FrameRunner
    {
        public void RemoveInactiveElements()
        {
            for (int i = 0; i < Count; i++)
            {
                if (!this[i].isActiveAndEnabled)
                {
                    RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
