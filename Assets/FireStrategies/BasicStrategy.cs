using Assets.Bullets;
using Assets.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.FireStrategies
{
    public class BasicStrategy : FireStrategy<BasicBullet>
    {
        public override LoopingFrameTimer FireTimer => new LoopingFrameTimer(0.5f);
    }
}
