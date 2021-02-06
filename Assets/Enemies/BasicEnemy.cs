using Assets.FireStrategies.EnemyFireStrategies;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public class BasicEnemy : PermanentVelocityEnemy
    {
        public override int BaseSpawnHealth => 100;
        public override float SpawnHealthScaleRate => 1.0f;

        public override EnemyFireStrategy FireStrategy { get; protected set; }
            = new BasicEnemyStrategy();
    }
}