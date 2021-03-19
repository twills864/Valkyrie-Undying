using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogUtilAssets;
using UnityEngine;

namespace Assets.Components
{
    public abstract class AttachableValkyrieComponent : Loggable
    {
        [SerializeField]
        protected ValkyrieSprite Host;

        public abstract void RunFrame(float deltaTime);
    }
}
