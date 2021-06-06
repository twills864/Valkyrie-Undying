using Assets.Bullets.PlayerBullets;
using Assets.FireStrategyManagers;
using Assets.ObjectPooling;
using Assets.Powerups;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <inheritdoc/>
    public class DefaultStrategy : PlayerFireStrategy<DefaultBullet>
    {
        public static int NumBulletsToGet { get; set; }

        public DefaultStrategy(DefaultBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
        {
            NumBulletsToGet = 1;
        }

        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios)
        {
            // Default ratio is 1, since the fire speed ratio is
            // fire speed compared to the default strategy.
            const float DefaultRatio = 1.0f;
            return DefaultRatio;
        }

        public override PlayerBullet[] GetBullets(int weaponLevel, Vector3 playerFirePos)
        {
            DefaultBullet[] bullets = PoolManager.Instance.BulletPool
                .GetMany<DefaultBullet>(NumBulletsToGet, playerFirePos, weaponLevel);

            GameManager.Instance.OnDefaultWeaponFire(bullets);

            return bullets;
        }
    }
}
