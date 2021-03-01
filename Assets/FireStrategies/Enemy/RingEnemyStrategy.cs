using Assets.Bullets.EnemyBullets;
using Assets.Util;

namespace Assets.FireStrategies.EnemyFireStrategies
{
    /// <inheritdoc/>
    public class RingEnemyStrategy : EnemyFireStrategy<RingEnemyBullet>
    {
        public override LoopingFrameTimer FireTimer { get; protected set; }
            = new LoopingFrameTimerWithRandomVariation(2.0f, 1.0f);
    }
}
