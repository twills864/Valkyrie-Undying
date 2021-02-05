using Assets.Bullets.EnemyBullets;
using Assets.EnemyBullets;
using Assets.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.FireStrategies.EnemyFireStrategies
{
    public class BasicEnemyStrategy : EnemyFireStrategy<BasicEnemyBullet>
    {
        public override LoopingFrameTimer FireTimer { get; protected set; }
            = new LoopingFrameTimerWithRandomVariation(3.0f, 0.5f);
    }
}
