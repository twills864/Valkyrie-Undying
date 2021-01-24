﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    public abstract class FrameTimerBase
    {
        public float Elapsed { get; protected set; }
        public float ActivationInterval { get; protected set; }
        public bool Activated { get; protected set; }

        public float TimeUntilActivation => ActivationInterval - Elapsed;
        public float RatioComplete => Elapsed / ActivationInterval;
        public float RatioRemaining => 1f - RatioComplete;

        public abstract void Increment(float deltaTime);

        public bool UpdateActivates(float deltaTime)
        {
            Increment(deltaTime);
            return Activated;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected string DebuggerDisplayBase
        {
            get
            {
                // Don't use .ToString("P0") to remove space before percent sign
                string percent = (RatioComplete * 100).ToString("0");
                string timeUntilActivation = TimeUntilActivation.ToString("0.##");
                string elapsed = Elapsed.ToString("0.##");
                string activationInterval = ActivationInterval.ToString("0.##");

                string ret = $"{{{percent}%}} [-{timeUntilActivation}] ({elapsed} -> {activationInterval})";
                return ret;
            }
        }
    }
}