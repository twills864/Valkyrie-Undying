using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;
using UnityEngine;

namespace Assets.GameTasks
{
    /// <inheritdoc/>
    public class Delay : FiniteTimeGameTask
    {
        public Delay(GameTaskRunner target, float duration) : base(target, duration)
        {
        }

        protected override void OnFiniteTaskFrameRun(float deltaTime)
        {

        }
    }
}
