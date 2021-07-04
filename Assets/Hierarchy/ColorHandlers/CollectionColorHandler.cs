using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Hierarchy.ColorHandlers
{
    /// <summary>
    /// Sets the colors of several ColorHandlers at once.
    /// </summary>
    /// <inheritdoc/>
    public class CollectionColorHandler : ColorHandler
    {
        private ColorHandler[] Handlers;

        public override Color Color
        {
            get => Handlers[0].Color;
            set
            {
                foreach (var handler in Handlers)
                    handler.Color = value;
            }
        }

        public override float Alpha
        {
            get => Color.a;
            set
            {
                foreach (var handler in Handlers)
                    handler.Alpha = value;
            }
        }

        public CollectionColorHandler(params ColorHandler[] handlers)
        {
            Handlers = handlers;
        }
    }
}
