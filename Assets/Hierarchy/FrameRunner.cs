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
    public abstract class FrameRunner : Loggable
    {
        public float TotalTime { get; set; }

        public abstract void RunFrame(float deltaTime);
    }
}