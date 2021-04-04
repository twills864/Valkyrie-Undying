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
        //private const float StartAlphaScale = 0.5f;

        private LineRenderer Line;

        public override Color Color
        {
            get => Line.startColor;
            set
            {
                if (Line.startColor != value)
                {
                    Line.endColor = value;

                    var startColor = new Color(value.r, value.g, value.b, value.a/* * StartAlphaScale*/);
                    Line.startColor = startColor;
                }
            }
        }

        public LineRendererColorHandler(LineRenderer line)
        {
            Line = line;
        }
    }
}
