using Assets.Bullets.EnemyBullets;
using Assets.Util;

namespace Assets.FireStrategies.EnemyFireStrategies
{
    /// <inheritdoc/>
    public class BasicEnemyStrategy : EnemyFireStrategy<BasicEnemyBullet>
    {
        public override LoopingFrameTimer FireTimer { get; protected set; }
            = new LoopingFrameTimerWithRandomVariation(3.0f, 0.5f);
    }
}
