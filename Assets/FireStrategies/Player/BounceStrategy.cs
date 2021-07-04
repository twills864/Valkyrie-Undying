using Assets.Bullets.PlayerBullets;
using Assets.FireStrategyManagers;
using Assets.UI.SpriteBank;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <summary>
    /// Fires a single Bounce bullet directly up.
    /// </summary>
    /// <inheritdoc/>
    public class BounceStrategy : PlayerFireStrategy<BounceBullet>
    {
        protected override Sprite GetPickupSprite(HeavyWeaponSpriteBank bank) => bank.Bounce;

        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios) => ratios.Bounce;

        //public override LoopingFrameTimer FireTimer { get; protected set; }
        //    = new LoopingFrameTimer(0.5f);

        private float BulletSpeed { get; set; }
        private Vector2 InitialBulletVelocity { get; set; }

        public BounceStrategy(BounceBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
        {
            BulletSpeed = bullet.Speed;
            InitialBulletVelocity = new Vector2(0, BulletSpeed);
        }

        public override PlayerBullet[] GetBullets(int weaponLevel, Vector3 playerFirePos)
        {
            PlayerBullet[] ret = GetBullets(weaponLevel, playerFirePos, InitialBulletVelocity);
            return ret;
        }
    }
}
