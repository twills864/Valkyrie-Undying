using Assets.Bullets.EnemyBullets;
using Assets.EnemyBullets;
using Assets.FireStrategies.EnemyFireStrategies;
using Assets.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.FireStrategies.EnemyFireStrategies
{
    public class DebugEnemyStrategy : EnemyFireStrategy<BasicEnemyBullet>
    {
        public override LoopingFrameTimer FireTimer { get; protected set; }
            = new LoopingFrameTimer(1.0f);
    }
}
