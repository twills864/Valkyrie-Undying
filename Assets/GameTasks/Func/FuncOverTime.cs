using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;
using UnityEngine;

namespace Assets.GameTasks
{
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