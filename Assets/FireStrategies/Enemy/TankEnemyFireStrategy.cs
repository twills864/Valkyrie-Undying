using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.EnemyBullets;
using Assets.EnemyBullets;
using Assets.ObjectPooling;
using Assets.UnityPrefabStructs;
using Assets.Util;
using UnityEngine;
using static Assets.Enemies.TankEnemy;

namespace Assets.FireStrategies.EnemyFireStrategies
{
    /// <summary>
    /// Fires a burst of Tank enemy bullets in quick succession.
    /// </summary>
    /// <inheritdoc/>
    public class TankEnemyFireStrategy : VariantLoopingEnemyFireStrategy<TankEnemyBullet>
    {
        private TankVariantFireSpeedExtra FireSpeedExtra;

        private float ReloadSpeed => FireSpeedExtra.ReloadSpeed;
        private int NumBulletsPerBurst => FireSpeedExtra.NumBulletsPerBurst;
        private float SpeedVariancePerBullet => FireSpeedExtra.SpeedVariancePerBullet;

        private int FireCounter;
        private float FireXFlip = 1.0f;

        public TankEnemyFireStrategy(VariantFireSpeed variantFireSpeed, TankVariantFireSpeedExtra variantFireSpeedExtra)
        : base(variantFireSpeed)
        {
            FireSpeedExtra = variantFireSpeedExtra;
            FireCounter = -NumBulletsPerBurst;
        }

        public override EnemyBullet[] GetBullets(Vector3 enemyFirePos)
        {
            var bullet = PoolManager.Instance.EnemyBulletPool.Get<TankEnemyBullet>(enemyFirePos);

            int varianceIndex = FireCounter + NumBulletsPerBurst;
            float variance = varianceIndex * SpeedVariancePerBullet;

            float velocityX = FireXFlip * RandomUtil.Float(0, variance);
            FireXFlip *= -1f;
            bullet.Velocity = new Vector2(velocityX, bullet.Speed);

            bullet.OnSpawn();

            FireCounter++;

            if (FireCounter != 0)
                FireTimer.ActivationInterval = FireSpeed;
            else
            {
                FireTimer.ActivationInterval = ReloadSpeed;
                FireCounter = -NumBulletsPerBurst;
            }

            var ret = new EnemyBullet[] { bullet };
            return ret;
        }

        protected override void OnReset()
        {
            FireCounter = -NumBulletsPerBurst;
        }
    }
}
