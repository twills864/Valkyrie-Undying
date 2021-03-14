﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.EnemyBullets;
using Assets.ObjectPooling;
using Assets.Util;

namespace Assets.FireStrategies.EnemyFireStrategies
{
    public class VariantLoopingEnemyFireStrategy<T> : EnemyFireStrategy<T>
        where T : EnemyBullet
    {
        protected float FireSpeed { get; set; }
        protected float Variance { get; set; }

        public VariantLoopingEnemyFireStrategy(float fireSpeed, float variance)
            : this(PoolManager.Instance.EnemyBulletPool.GetPrefab<T>(), fireSpeed, variance)
        {
        }

        public VariantLoopingEnemyFireStrategy(T bulletPrefab, float fireSpeed, float variance) : base(bulletPrefab)
        {
            FireSpeed = fireSpeed;
            Variance = variance;

            FireTimer = InitialFireTimer();
        }

        protected LoopingFrameTimer InitialFireTimer()
            => new LoopingFrameTimerWithRandomVariation(FireSpeed, Variance);

        protected override EnemyFireStrategy CloneSelf()
        {
            throw new NotImplementedException();
        }
    }
}