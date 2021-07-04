using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Hierarchy.ColorHandlers
{
    /// <summary>
    /// Sets the color of a given LineRenderer.
    /// </summary>
    /// <inheritdoc/>
    public class LineRendererColorHandler : ColorHandler
    {
        private LineRenderer Line;

        public override Color Color
        {
            get => Line.startColor;
            set
            {
                if (Line.startColor != value)
                {
                    Line.endColor = value;
                    Line.startColor = value;
                }
            }
        }

        public LineRendererColorHandler(LineRenderer line)
        {
            Line = line;
        }
    }
}
