using Assets.Bullets.PlayerBullets;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <inheritdoc/>
    public class GatlingStrategy : PlayerFireStrategy<GatlingBullet>
    {
        public override LoopingFrameTimer FireTimer { get; protected set; }
            = new LoopingFrameTimer(0.2f);

        public GatlingStrategy(GatlingBullet bullet) : base(bullet)
        {
        }

        public override PlayerBullet[] GetBullets(int weaponLevel, Vector2 playerFirePos)
        {
            var ret = base.GetBullets(weaponLevel, playerFirePos);

            GatlingBullet bullet = (GatlingBullet) ret[0];

            float offsetBound = 10f * Mathf.Deg2Rad;
            float offest = RandomUtil.Float(-offsetBound, offsetBound);
            bullet.RayCastUp(offest);

            return ret;
        }
    }
}
