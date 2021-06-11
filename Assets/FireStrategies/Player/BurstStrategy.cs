using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Util;
using Assets.ObjectPooling;
using UnityEngine;
using Assets.FireStrategyManagers;
using Assets.UI.SpriteBank;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <inheritdoc/>
    public class BurstStrategy : PlayerFireStrategy<BurstBullet>
    {
        protected override Sprite GetPickupSprite(HeavyWeaponSpriteBank bank) => bank.Burst;
        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio playerRatios)
            => playerRatios.Burst.FireRatio;

        private const int BulletsInFirstBurst = 2;
        private int FireCounter = -BulletsInFirstBurst;

        private float BulletVelocityY;
        private float BulletSpreadX;
        private float BulletSpreadY;

        private float FireActivationInterval { get; set; }
        private float ReloadActivationInterval { get; set; }

        public BurstStrategy(BurstBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
        {
            BulletVelocityY = bullet.BulletVelocityY;
            BulletSpreadX = bullet.BulletSpreadX;
            BulletSpreadY = bullet.BulletSpreadY;

            FireActivationInterval = manager.BaseFireSpeed * manager.PlayerRatios.Burst.FireRatio;
            ReloadActivationInterval = manager.BaseFireSpeed * manager.PlayerRatios.Burst.ReloadRatio;
        }

        private Vector2 NewVelocity()
        {
            float x = RandomUtil.Float(-BulletSpreadX, BulletSpreadX);
            float y = BulletVelocityY + RandomUtil.Float(-BulletSpreadY, BulletSpreadY);
            Vector2 ret = new Vector2(x, y);
            return ret;
        }
        public override PlayerBullet[] GetBullets(int weaponLevel, Vector3 playerFirePos)
        {
            PlayerBullet[] ret = base.GetBullets(weaponLevel, playerFirePos, NewVelocity());

            FireCounter++;
            if (FireCounter != weaponLevel)
            {
                FireTimer.ActivationInterval = FireActivationInterval;
            }
            else
            {
                FireCounter = -BulletsInFirstBurst;
                FireTimer.ActivationInterval = ReloadActivationInterval;
            }

            return ret;
        }
    }
}
