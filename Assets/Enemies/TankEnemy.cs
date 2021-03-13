using Assets.EnemyBullets;
using Assets.FireStrategies;
using Assets.FireStrategies.EnemyFireStrategies;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public class TankEnemy : PermanentVelocityEnemy
    {
        //public override float VictimMarkerDistance => 1.0f;
        protected override EnemyFireStrategy InitialFireStrategy()
            => new VariantLoopingEnemyFireStrategy<TankEnemyBullet>(FireSpeed, FireSpeed);
    }
}