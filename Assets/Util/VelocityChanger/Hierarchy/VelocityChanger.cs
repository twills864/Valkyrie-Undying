using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public abstract class VelocityChanger : FrameRunner
    {
        protected ManagedVelocityObject Target { get; set; }
        public VelocityChanger(ManagedVelocityObject target)
        {
            Target = target;
        }
    }
}
