using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogUtilAssets;
using UnityEngine;

namespace Assets.Components
{
    /// <summary>
    /// Represents a component that will handle common game functionality
    /// for a specified host ValkyrieSprite, and will be managed by the
    /// host itself, instead of being attached to a GameObject in the Unity editor.
    /// </summary>
    /// <inheritdoc/>
    public abstract class ValkyrieComponent
    {
        protected ValkyrieSprite Host { get; private set; }

        public ValkyrieComponent(ValkyrieSprite host)
        {
            Host = host;
        }

        public abstract void RunFrame(float deltaTime, float realDeltaTime);
    }
}
