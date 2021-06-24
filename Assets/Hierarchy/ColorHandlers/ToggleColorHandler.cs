using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Hierarchy.ColorHandlers
{
    public class ToggleColorHandler : ColorHandler
    {
        private Toggle Toggle;
        private Color NormalColor;

        public override Color Color
        {
            get => Toggle.colors.normalColor;
            set
            {
                if (Toggle.colors.normalColor != value)
                {
                    ColorBlock colorBlock = Toggle.colors;
                    colorBlock.normalColor = value;
                    Toggle.colors = colorBlock;
                }
            }
        }

        public ToggleColorHandler(Toggle toggle)
        {
            Toggle = toggle;
            NormalColor = toggle.colors.normalColor;
        }
    }
}
