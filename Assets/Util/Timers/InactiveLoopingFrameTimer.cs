using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    /// <summary>
    /// Represents a FrameTimer that will never activate.
    /// </summary>
    public class InactiveLoopingFrameTimer : LoopingFrameTimer
    {
        public override bool Activated { get => false; protected set { } }

        public InactiveLoopingFrameTimer() : base(1.0f)
        {
            Activated = false;
        }

        public override void Increment(float deltaTime)
        {
            return;
        }

        public override string ToString() => "(InactiveLoopingFrameTimer)";
    }
}
