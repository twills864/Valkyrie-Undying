using Assets.Bullets.EnemyBullets;
using Assets.FireStrategyManagers;
using Assets.Util;

namespace Assets.FireStrategies.EnemyFireStrategies
{
    /// <inheritdoc/>
    public class InactiveEnemyStrategy : EnemyFireStrategy<BasicEnemyBullet>
    {
        public InactiveEnemyStrategy() : base(null)
        {
            FireTimer = new InactiveLoopingFrameTimer();
        }

        protected override EnemyFireStrategy CloneSelf()
        {
            throw new System.NotImplementedException();
        }
    }
}
