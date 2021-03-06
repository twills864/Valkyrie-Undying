using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Hierarchy.ColorHandlers
{
    /// <summary>
    /// Sets the color of a given GameObject.
    /// </summary>
    public abstract class ColorHandler
    {
        public abstract Color Color { get; set; }

        public float Alpha
        {
            get => Color.a;
            set
            {
                Color color = Color;
                color.a = value;
                Color = color;
            }
        }
    }
}
