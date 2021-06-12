using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Hierarchy.ColorHandlers
{
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
