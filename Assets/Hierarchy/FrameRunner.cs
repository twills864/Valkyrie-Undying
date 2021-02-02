using Assets.Util;
using LogUtilAssets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    /// <summary>
    /// Represents an object that will manually handle frame updates
    /// instead of relying on Unity's implementation of Update().
    /// Useful for controlling delta times.
    /// </summary>
    public abstract class FrameRunner : Loggable
    {
        public float TotalTime { get; set; }

        public abstract void RunFrame(float deltaTime);
    }
}