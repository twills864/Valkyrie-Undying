using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogUtilAssets;
using UnityEngine;

namespace Assets.Components
{
    public abstract class ValkyrieComponent
    {
        protected ValkyrieSprite Host { get; private set; }

        public ValkyrieComponent(ValkyrieSprite host)
        {
            Host = host;
        }

        public abstract void RunFrame(float deltaTime);
    }
}
