using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    /// <summary>
    /// Contains an array of Actions that each represent one of many possible
    /// behaviors a given GameObject should be running on a given frame.
    /// For example, an enemy may have vastly different functionality when it's
    /// first flying in to the screen compared to when it is actively attacking
    /// the player.
    /// </summary>
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
