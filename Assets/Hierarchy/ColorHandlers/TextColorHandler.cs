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
    /// Sets the color of a given Unity Text.
    /// </summary>
    /// <inheritdoc/>
    public class TextColorHandler : ColorHandler
    {
        private Text Text;

        public override Color Color
        {
            get => Text.color;
            set
            {
                if (Text.color != value)
                    Text.color = value;
            }
        }

        public TextColorHandler(Text text)
        {
            Text = text;
        }
    }
}
