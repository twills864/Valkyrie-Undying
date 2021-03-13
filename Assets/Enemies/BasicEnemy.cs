using Assets.Bullets.EnemyBullets;
using Assets.FireStrategies.EnemyFireStrategies;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public class BasicEnemy : PermanentVelocityEnemy
    {
        protected override EnemyFireStrategy InitialFireStrategy()
            => new VariantLoopingEnemyFireStrategy<BasicEnemyBullet>(FireSpeed, FireSpeedVariance);
    }
}