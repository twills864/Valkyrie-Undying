using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets;
using UnityEngine;

namespace Assets.Hierarchy.ColorHandlers
{
    public class BulletTrailColorHandler : ColorHandler
    {
        private static Color DefaultColor => Color.white;

        private Bullet Bullet;
        private TrailRenderer Trail => Bullet.CurrentBulletTrail?.Trail;

        public override Color Color
        {
            get => Trail?.startColor ?? DefaultColor;
            set
            {
                var trail = Trail;
                if (trail != null)
                {
                    if (trail.startColor != value)
                    {
                        trail.endColor = value;
                        trail.startColor = value;
                    }
                }
            }
        }

        public BulletTrailColorHandler(Bullet bullet)
        {
            Bullet = bullet;
        }
    }
}
