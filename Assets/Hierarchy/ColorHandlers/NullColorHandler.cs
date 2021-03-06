using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Hierarchy.ColorHandlers
{
    /// <summary>
    /// A ColorHandler subclass that deliberately does nothing.
    /// Used for invisible GameObjects.
    /// </summary>
    public class NullColorHandler : ColorHandler
    {
        private SpriteRenderer Sprite;

        public override Color Color { get; set; }
    }
}
