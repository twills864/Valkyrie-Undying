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
    /// Represents a component that can be attached to a
    /// GameObject in the Unity editor.
    ///
    /// Currently unused.
    /// </summary>
    /// <inheritdoc/>
    public abstract class AttachableValkyrieComponent : Loggable
    {
        [SerializeField]
        protected ValkyrieSprite Host;

        public abstract void RunFrame(float deltaTime);
    }
}
