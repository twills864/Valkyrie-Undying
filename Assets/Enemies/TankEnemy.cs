using Assets.FireStrategies;
using Assets.FireStrategies.EnemyFireStrategies;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public class TankEnemy : PermanentVelocityEnemy
    {
        public override int BaseSpawnHealth => 250;
        public override float SpawnHealthScaleRate => 2.5f;

        public override EnemyFireStrategy FireStrategy { get; protected set; }
            = new TankEnemyStrategy();

        public override float VictimMarkerDistance => 1.0f;
    }
}