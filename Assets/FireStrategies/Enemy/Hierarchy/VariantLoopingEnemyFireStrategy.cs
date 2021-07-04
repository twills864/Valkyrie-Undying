using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.EnemyBullets;
using Assets.ObjectPooling;
using Assets.UnityPrefabStructs;
using Assets.Util;

namespace Assets.FireStrategies.EnemyFireStrategies
{
    /// <summary>
    /// A fire strategy that periodically fires a specified enemy bullet
    /// with a given fixed delay and random delay variance.
    /// </summary>
    /// <typeparam name="T">The type of enemy bullet to fire.</typeparam>
    /// <inheritdoc/>
    public abstract class VariantLoopingEnemyFireStrategy<T> : EnemyFireStrategy<T>
        where T : EnemyBullet
    {
        protected float FireSpeed { get; set; }
        protected float Variance { get; set; }

        public VariantLoopingEnemyFireStrategy(VariantFireSpeed variantFireSpeed)
            : this(PoolManager.Instance.EnemyBulletPool.GetPrefab<T>(), variantFireSpeed.FireSpeed, variantFireSpeed.FireSpeedVariance)
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
    }
}
