using Assets.Bullets.PlayerBullets;
using Assets.FireStrategyManagers;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <summary>
    /// Two-step fire strategy:
    /// 1. Spawn a Battle-Frenzied Guillotine spawner that acts as an outline for the laser about to fire.
    /// 2. Fire a Battle-Frenzied Guillotine.
    /// </summary>
    /// <inheritdoc/>
    public class BfgStrategy : PlayerFireStrategy<BfgBullet>
    {
        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios) => ratios.Bfg.ChargeRatio;

        protected override string CalculateFireStrategyName() => "Battle-Frenzied Guillotine";

        private BulletPoolList PoolList { get; set; }
        private float ChargeActivationInterval { get; set; }
        private float LaserActivationInterval { get; set; }

        public BfgStrategy(BfgBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
        {
            PoolList = PoolManager.Instance.BulletPool;

            ChargeActivationInterval = manager.BaseFireSpeed * manager.PlayerRatios.Bfg.ChargeRatio;
            LaserActivationInterval = manager.BaseFireSpeed * manager.PlayerRatios.Bfg.LaserRatio;
        }

        public override PlayerBullet[] GetBullets(int weaponLevel, Vector3 playerFirePos)
        {
            PlayerBullet[] ret;

            if (BfgBulletSpawner.TryGetInactiveSpawner(out BfgBulletSpawner spawner))
            {
                BfgBulletSpawner bullet = PoolList.Get<BfgBulletSpawner>(playerFirePos, weaponLevel);
                //bullet.FallbackDeactivationTime = LaserActivationInterval;

                ret = new PlayerBullet[]
                {
                    bullet
                };

                FireTimer.ActivationInterval = LaserActivationInterval;
            }
            else
            {
                ret = base.GetBullets(weaponLevel, playerFirePos);

                // Don't need to call DeactivateSelft() - SetActive(false) is enough.
                spawner.gameObject.SetActive(false);

                FireTimer.ActivationInterval = ChargeActivationInterval;
            }

            return ret;
        }
    }
}
