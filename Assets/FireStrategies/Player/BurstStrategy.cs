using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Util;
using Assets.ObjectPooling;
using UnityEngine;
using Assets.FireStrategyManagers;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <inheritdoc/>
    public class BurstStrategy : PlayerFireStrategy<BurstBullet>
    {
        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio playerRatios)
            => playerRatios.Burst;

        private const int BulletsInFirstBurst = 3;
        private const int AdditionalBulletsInMaxBurst = GameConstants.MaxWeaponLevel + 1;
        private int FireCounter = -BulletsInFirstBurst;

        private float BulletVelocityY;
        private float BulletSpreadX;
        private float BulletSpreadY;

        public BurstStrategy(BurstBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
        {
            BulletVelocityY = bullet.BulletVelocityY;
            BulletSpreadX = bullet.BulletSpreadX;
            BulletSpreadY = bullet.BulletSpreadY;
        }

        private Vector2 NewVelocity()
        {
            float x = RandomUtil.Float(-BulletSpreadX, BulletSpreadX);
            float y = BulletVelocityY + RandomUtil.Float(-BulletSpreadY, BulletSpreadY);
            Vector2 ret = new Vector2(x, y);
            return ret;
        }
        public override PlayerBullet[] GetBullets(int weaponLevel, Vector2 playerFirePos)
        {
            PlayerBullet[] ret;

            if(weaponLevel == GameConstants.MaxWeaponLevel
                || FireCounter < weaponLevel)
            {
                ret = base.GetBullets(weaponLevel, playerFirePos, NewVelocity());
            }
            else
                ret = new PlayerBullet[0];

            FireCounter++;
            if(FireCounter == AdditionalBulletsInMaxBurst)
                FireCounter = -BulletsInFirstBurst;

            return ret;
        }
    }
}
