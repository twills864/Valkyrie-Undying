using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets;
using Assets.UI;
using Assets.Util;
using UnityEngine;

namespace Assets.Hierarchy.ColorHandlers
{
    public class BulletTrailColorHandler : ColorHandler
    {
        private TrailRenderer Trail;

        public override Color Color
        {
            get => Trail.startColor;
            set
            {
                Color newValue = AdjustColor(value);
                if (Trail.startColor != newValue)
                {
                    Trail.startColor = newValue;
                    Trail.endColor = newValue.WithAlpha(0f);
                }
            }
        }

        public BulletTrailColorHandler(TrailRenderer trail)
        {
            Trail = trail;
        }

        private Color AdjustColor(Color color)
        {
            return SlightlyTransparent(color);
        }

        private Color SlightlyTransparent(Color color)
        {
            const float Transparency = 0.8f;
            return color.WithAlpha(Transparency);
        }

        [Obsolete("Looks awful")]
        private Color SlightlyGrayScale(Color color)
        {
            const float GrayWeight = 0.3f;
            const float ColorWeight = 1f - GrayWeight;

            float gray = color.grayscale;
            float grayWeighted = gray * GrayWeight;

            float r = (ColorWeight * color.r) + (grayWeighted);
            float g = (ColorWeight * color.g) + (grayWeighted);
            float b = (ColorWeight * color.b) + (grayWeighted);

            Color adjusted = new Color(r, g, b, color.a);
            return adjusted;
        }
    }
}
