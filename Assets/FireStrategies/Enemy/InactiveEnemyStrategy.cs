using Assets.Bullets.EnemyBullets;
using Assets.Util;

namespace Assets.FireStrategies.EnemyFireStrategies
{
    /// <inheritdoc/>
    public class InactiveEnemyStrategy : EnemyFireStrategy<BasicEnemyBullet>
    {
        public override LoopingFrameTimer FireTimer { get; protected set; }
            = new InactiveLoopingFrameTimer();
    }
}
