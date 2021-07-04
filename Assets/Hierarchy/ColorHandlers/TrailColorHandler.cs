using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Hierarchy.ColorHandlers
{
    /// <summary>
    /// Sets the color of a given TrailRenderer.
    /// Designed for use by TrailRenderers not associated with the Bullet Trail system.
    /// </summary>
    /// <inheritdoc/>
    public class TrailColorHandler : ColorHandler
    {
        private TrailRenderer Trail;

        public override Color Color
        {
            get => Trail.startColor;
            set
            {
                if (Trail.startColor != value)
                {
                    Trail.startColor = value;
                    Trail.endColor = value;
                }
            }
        }

        public TrailColorHandler(TrailRenderer trail)
        {
            Trail = trail;
        }
    }
}
