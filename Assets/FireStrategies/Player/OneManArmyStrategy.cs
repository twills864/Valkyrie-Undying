using System.Linq;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.FireStrategyManagers;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <summary>
    ///
    /// </summary>
    /// <inheritdoc/>
    public class OneManArmyStrategy : PlayerFireStrategy<OneManArmyBullet>
    {
        public OneManArmyStrategy(OneManArmyBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
        {
        }

        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios)
            => ratios.OneManArmy;

        public override PlayerBullet[] GetBullets(int weaponLevel, Vector3 playerFirePos)
        {
            int numbullets = weaponLevel + 2;

            var ret = PoolManager.Instance.BulletPool.GetMany<OneManArmyBullet>(numbullets);
            float offsetX = ret[0].OffsetX;

            Vector3 spawn = playerFirePos;
            spawn.x -= (offsetX * (numbullets-1) * 0.5f);
            for (int i = 0; i < numbullets; i++)
            {
                ret[i].transform.position = spawn;
                spawn.x += offsetX;
            }

            return ret;
        }
    }
}
