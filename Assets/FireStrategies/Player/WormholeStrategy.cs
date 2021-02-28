using Assets.Bullets.PlayerBullets;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <inheritdoc/>
    public class WormholeStrategy : PlayerFireStrategy<WormholeBullet>
    {
        public override LoopingFrameTimer FireTimer { get; protected set; }
            = new LoopingFrameTimer(0.9f);


        public WormholeStrategy(WormholeBullet bullet) : base(bullet)
        {
        }
    }
}
