using Assets.EnemyBullets;
using Assets.FireStrategies.EnemyFireStrategies;
using Assets.Util;

namespace Assets.FireStrategies
{
    /// <inheritdoc/>
    public class TankEnemyStrategy : EnemyFireStrategy<TankEnemyBullet>
    {
        public override LoopingFrameTimer FireTimer { get; protected set; }
            = new LoopingFrameTimerWithRandomVariation(6.0f, 0.5f);
    }
}
