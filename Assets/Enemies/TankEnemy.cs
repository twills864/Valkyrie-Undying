using Assets.EnemyBullets;
using Assets.FireStrategies;
using Assets.FireStrategies.EnemyFireStrategies;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public class TankEnemy : PermanentVelocityEnemy
    {
        protected override EnemyFireStrategy InitialFireStrategy()
            => new VariantLoopingEnemyFireStrategy<TankEnemyBullet>(VariantFireSpeed);
    }
}