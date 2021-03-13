using Assets.Bullets.EnemyBullets;
using Assets.FireStrategyManagers;
using Assets.Util;

namespace Assets.FireStrategies.EnemyFireStrategies
{
    /// <inheritdoc/>
    public class InactiveEnemyStrategy : VariantLoopingEnemyFireStrategy<BasicEnemyBullet>
    {
        public InactiveEnemyStrategy() : base(null, 1.0f, 0.0f)
        {
        }

        protected override EnemyFireStrategy CloneSelf()
        {
            throw new System.NotImplementedException();
        }
    }
}
