﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;
using UnityEngine;

namespace Assets.GameTasks
{
    /// <summary>
    /// Deliberately does nothing over a specified period of time.
    /// Designed to be used as part of a Sequence which might require
    /// a delay before activating some functionality.
    /// </summary>
    /// <inheritdoc/>
    public class Delay : FiniteTimeGameTask
    {
        public Delay(ValkyrieSprite target, float duration) : base(target, duration)
        {
        }

        protected override void OnFiniteTaskFrameRun(float deltaTime)
        {

        }

        public static Delay Default() => new Delay(null, 1.0f);
    }
}
