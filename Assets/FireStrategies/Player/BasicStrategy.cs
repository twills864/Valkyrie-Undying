﻿using Assets.Bullets;
using Assets.Util;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <inheritdoc/>
    public class BasicStrategy : PlayerFireStrategy<BasicBullet>
    {
        public override LoopingFrameTimer FireTimer { get; protected set; }
            = new LoopingFrameTimer(0.5f);
    }
}
