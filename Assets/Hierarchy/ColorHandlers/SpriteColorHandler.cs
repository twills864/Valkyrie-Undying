using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Hierarchy.ColorHandlers
{
    /// <summary>
    /// Sets the color of a given SpriteRenderer.
    /// </summary>
    /// <inheritdoc/>
    public class SpriteColorHandler : ColorHandler
    {
        private SpriteRenderer Sprite;

        public override Color Color
        {
            get => Sprite.color;
            set => Sprite.color = value;
        }

        public SpriteColorHandler(SpriteRenderer sprite)
        {
            Sprite = sprite;
        }
    }
}
