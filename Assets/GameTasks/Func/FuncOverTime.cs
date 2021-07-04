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
    /// Runs a given Action every frame the GameTask is active.
    /// </summary>
    /// <inheritdoc/>
    public class FuncOverTime : FiniteTimeGameTask
    {
        private Action Action { get; set; }

        public FuncOverTime(ValkyrieSprite target, Action action, float duration) : base(target, duration)
        {
            Action = action;
        }

        protected override void OnFiniteTaskFrameRun(float deltaTime)
        {
            Action();
        }
    }
}