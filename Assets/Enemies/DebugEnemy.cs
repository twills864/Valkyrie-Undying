using Assets.FireStrategies.EnemyFireStrategies;
using Assets.Util;
using UnityEngine;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public class DebugEnemy : Enemy
    {
        public override int BaseSpawnHealth => 100000;
        public override float SpawnHealthScaleRate => 1.0f;
        protected override bool ShouldDeactivateOnDestructor => false;

        public override EnemyFireStrategy FireStrategy { get; protected set; }
            = new DebugEnemyStrategy();

        protected override void OnEnemyInit()
        {
            base.OnActivate();
        }
    }
}