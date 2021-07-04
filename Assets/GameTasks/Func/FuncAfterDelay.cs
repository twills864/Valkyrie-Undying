using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;
using UnityEngine;

namespace Assets.GameTasks
{
    /// <summary>
    /// Runs a specified Action after a given delay.
    /// Acts as a simplified version of a Sequence containing a Delay and a GameTaskFunc.
    /// </summary>
    /// <inheritdoc/>
    public class FuncAfterDelay : FiniteTimeGameTask
    {
        private Action Func;

        public FuncAfterDelay(ValkyrieSprite target, Action func, float duration) : base(target, duration)
        {
            Func = func;
        }

        protected override void OnFiniteTaskFrameRun(float deltaTime)
        {
            if (Timer.Activated)
                Func();
        }

        public static FuncAfterDelay DeactivateSelfAfterDelay(PooledObject runner, float delay)
        {
            Action deactivateSelf = runner.DeactivateSelf;
            FuncAfterDelay ret = new FuncAfterDelay(runner, deactivateSelf, delay);
            return ret;
        }
    }
}
