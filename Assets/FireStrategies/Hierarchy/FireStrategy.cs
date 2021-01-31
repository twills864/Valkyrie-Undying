using Assets.Bullets;
using Assets.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FireStrategies
{
    public abstract class FireStrategy
    {
        private Bullet BulletPrefab { get; set; }
        private FrameTimer Timer { get; set; }

        protected abstract FrameTimer DefaultTimer { get; }

        public FireStrategy(Bullet bulletPrefab)
        {
            BulletPrefab = bulletPrefab;
            Timer = DefaultTimer;
        }


    }
}
