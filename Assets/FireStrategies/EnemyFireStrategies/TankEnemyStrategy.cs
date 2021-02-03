using Assets.EnemyBullets;
using Assets.EnemyFireStrategies;
using Assets.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.FireStrategies
{
    public class TankEnemyStrategy : EnemyFireStrategy<TankEnemyBullet>
    {
        public override LoopingFrameTimer FireTimer => new LoopingFrameTimerWithRandomVariation(3.0f, 0.5f);
    }
}
