using Assets.Bullets;
using Assets.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    public class BasicStrategy : PlayerFireStrategy<BasicBullet>
    {
        public override LoopingFrameTimer FireTimer { get; protected set; }
            = new LoopingFrameTimer(0.5f);
    }
}
