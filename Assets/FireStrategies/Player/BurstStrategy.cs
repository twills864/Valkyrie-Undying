using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Util;
using Assets.Util.ObjectPooling;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <inheritdoc/>
    public class BurstStrategy : PlayerFireStrategy<BurstBullet>
    {
        public override LoopingFrameTimer FireTimer { get; protected set; }
            = new LoopingFrameTimer(0.1f);

        private const int BulletsInFirstBurst = 3;
        private const int AdditionalBulletsInMaxBurst = GameConstants.MaxWeaponLevel + 1;
        private int FireCounter = -BulletsInFirstBurst;

        private float BulletVelocityY;
        private float BulletSpreadX;
        private float BulletSpreadY;

        public BurstStrategy(BurstBullet bullet)
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
