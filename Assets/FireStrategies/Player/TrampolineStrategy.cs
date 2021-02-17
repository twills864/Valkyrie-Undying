using Assets.Bullets.PlayerBullets;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <inheritdoc/>
    public class TrampolineStrategy : PlayerFireStrategy<TrampolineBullet>
    {
        public override LoopingFrameTimer FireTimer { get; protected set; }
            = new LoopingFrameTimer(0.5f);

        private float BulletSpeed { get; set; }
        private Vector2 InitialBulletVelocity { get; set; }

        public TrampolineStrategy(TrampolineBullet bullet) : base(bullet)
        {
            BulletSpeed = bullet.Speed;
            InitialBulletVelocity = new Vector2(0, BulletSpeed);
        }

        public override PlayerBullet[] GetBullets(int weaponLevel, Vector2 playerFirePos)
        {
            PlayerBullet[] ret = GetBullets(weaponLevel, playerFirePos, InitialBulletVelocity,
                x => x.OnSpawn(weaponLevel, playerFirePos));
            return ret;
        }
    }
}
