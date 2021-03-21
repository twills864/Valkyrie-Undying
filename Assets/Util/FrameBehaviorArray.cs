using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public struct FrameBehaviorArray
    {
        public int BehaviorIndex;
        public Action<float>[] Behaviors;

        public Action<float> CurrentBehavior => Behaviors[BehaviorIndex];

        public FrameBehaviorArray(params Action<float>[] behaviors)
        {
            BehaviorIndex = 0;
            Behaviors = behaviors;
        }

        public static implicit operator FrameBehaviorArray(Action<float>[] actions)
        {
            var ret = new FrameBehaviorArray();
            ret.Behaviors = actions;
            return ret;
        }

        public void ResetSelf() => BehaviorIndex = 0;
    }
}
