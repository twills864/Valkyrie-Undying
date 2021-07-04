using Assets.Bullets.EnemyBullets;
using Assets.FireStrategyManagers;
using Assets.Util;

namespace Assets.FireStrategies.EnemyFireStrategies
{
    /// <summary>
    /// Represents a fire strategy that will never activate or attempt to fire.
    /// </summary>
    /// <inheritdoc/>
    public class InactiveEnemyStrategy : EnemyFireStrategy<BasicEnemyBullet>
    {
        public InactiveEnemyStrategy() : base(null)
        {
            FireTimer = new InactiveLoopingFrameTimer();
        }
    }
}
