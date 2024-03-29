﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;
using UnityEngine;

namespace Assets.Hierarchy.ColorHandlers
{
    /// <summary>
    /// Sets the color of a given TrailRenderer, while also
    /// applying a fade-out effect.
    /// </summary>
    /// <inheritdoc/>
    public class TrailFadeOutColorHandler : ColorHandler
    {
        private TrailRenderer Trail;

        public override Color Color
        {
            get => Trail.startColor;
            set
            {
                if (Trail.startColor != value)
                {
                    Trail.startColor = value;
                    Trail.endColor = value.WithAlpha(0f);
                }
            }
        }

        public TrailFadeOutColorHandler(TrailRenderer trail)
        {
            Trail = trail;
        }
    }
}
