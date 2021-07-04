using System.Linq;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.FireStrategyManagers;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.UI.SpriteBank;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <summary>
    /// Fires many One Man Army bullets along a horizontal line.
    /// The number of bullets in this line increases with the bullet level.
    /// At the maximum bullet level, the horizontal line on which the bullets spawn
    /// is guaranteed to remain in bounds, ensuring that each bullet stays in bounds
    /// even if the player is near the edge of the screen.
    /// </summary>
    /// <inheritdoc/>
    public class OneManArmyStrategy : PlayerFireStrategy<OneManArmyBullet>
    {
        protected override string CalculateFireStrategyName() => "One-Man Army";

        private float WidthHalf { get; set; }

        public OneManArmyStrategy(OneManArmyBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
        {
            var renderer = bullet.GetComponent<Renderer>();
            WidthHalf = renderer.bounds.size.x * 0.5f;
        }

        protected override Sprite GetPickupSprite(HeavyWeaponSpriteBank bank) => bank.OneManArmy;

        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios)
            => ratios.OneManArmy;

        public override PlayerBullet[] GetBullets(int weaponLevel, Vector3 playerFirePos)
        {
            int numBullets = weaponLevel + 2;

            var ret = PoolManager.Instance.BulletPool.GetMany<OneManArmyBullet>(numBullets);
            float offsetX = ret[0].OffsetX;

            Vector3 spawn = playerFirePos;
            spawn.x -= (offsetX * (numBullets-1) * 0.5f);

            if (weaponLevel != GameConstants.MaxWeaponLevel)
            {
                for (int i = 0; i < numBullets; i++)
                {
                    OneManArmyBullet bullet = ret[i];
                    bullet.transform.position = spawn;

                    if (!SpaceUtil.PointIsInBounds(spawn))
                        bullet.RunTask(GameTaskFunc.DeactivateSelf(bullet));

                    spawn.x += offsetX;
                    bullet.OnSpawn();
                }
            }
            else
            {
                bool left = playerFirePos.x < SpaceUtil.WorldMap.Center.x;
                if (left)
                {
                    if (!SpaceUtil.XCoordinateIsInBounds(spawn.x))
                        spawn.x = SpaceUtil.WorldMap.Left.x + WidthHalf;
                }
                else
                {
                    float finalX = spawn.x + ((numBullets-1) * offsetX);
                    if(!SpaceUtil.XCoordinateIsInBounds(finalX))
                    {
                        spawn.x = SpaceUtil.WorldMap.Right.x - WidthHalf;
                        offsetX *= -1f;
                    }
                }

                for (int i = 0; i < numBullets; i++)
                {
                    OneManArmyBullet bullet = ret[i];

                    bullet.transform.position = spawn;
                    spawn.x += offsetX;
                    bullet.OnSpawn();
                }
            }

            return ret;
        }
    }
}
