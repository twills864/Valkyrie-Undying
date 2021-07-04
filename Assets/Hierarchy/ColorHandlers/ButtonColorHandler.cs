using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Hierarchy.ColorHandlers
{
    /// <summary>
    /// Sets the color of a given Unity button.
    /// </summary>
    /// <inheritdoc/>
    public class ButtonColorHandler : ColorHandler
    {
        private Button Button;

        public override Color Color
        {
            get => Button.image.color;
            set
            {
                if (Button.image.color != value)
                    Button.image.color = value;
            }
        }

        public ButtonColorHandler(Button button)
        {
            Button = button;
        }
    }
}
