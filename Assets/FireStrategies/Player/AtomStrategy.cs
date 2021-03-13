using Assets.Bullets.PlayerBullets;
using Assets.FireStrategyManagers;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <inheritdoc/>
    public class AtomStrategy : PlayerFireStrategy<AtomBullet>
    {
        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios) => ratios.Atom;

        private float BulletSpeed { get; set; }
        private Vector2 InitialBulletVelocity { get; set; }

        //public override LoopingFrameTimer FireTimer { get; protected set; }
        //    = new LoopingFrameTimer(0.4f);

        public AtomStrategy(AtomBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
        {
            BulletSpeed = bullet.Speed;
            InitialBulletVelocity = new Vector2(0, BulletSpeed);
        }

        public override PlayerBullet[] GetBullets(int weaponLevel, Vector2 playerFirePos)
        {
            PlayerBullet[] ret = GetBullets(weaponLevel, playerFirePos, InitialBulletVelocity);
            return ret;
        }
    }
}