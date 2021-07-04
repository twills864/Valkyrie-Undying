using Assets.Bullets.PlayerBullets;
using Assets.FireStrategyManagers;
using Assets.UI.SpriteBank;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <summary>
    /// Fires a single Trampoline bullet directly up.
    /// </summary>
    /// <inheritdoc/>
    public class TrampolineStrategy : PlayerFireStrategy<TrampolineBullet>
    {
        protected override Sprite GetPickupSprite(HeavyWeaponSpriteBank bank) => bank.Trampoline;

        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios) => ratios.Trampoline;
        //public override LoopingFrameTimer FireTimer { get; protected set; }
        //    = new LoopingFrameTimer(0.5f);

        private float BulletSpeed { get; set; }
        private Vector2 InitialBulletVelocity { get; set; }

        public TrampolineStrategy(TrampolineBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
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
