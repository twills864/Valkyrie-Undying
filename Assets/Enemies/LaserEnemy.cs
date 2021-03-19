using Assets.Bullets.EnemyBullets;
using Assets.Components;
using Assets.FireStrategies.EnemyFireStrategies;
using Assets.Util;
using UnityEngine;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public class LaserEnemy : Enemy
    {
        private PositionRotator Rotator { get; set; }

        protected override EnemyFireStrategy InitialFireStrategy()
            => new VariantLoopingEnemyFireStrategy<BasicEnemyBullet>(1.0f, 1.0f);
        protected override void OnEnemyInit()
        {
            Rotator = new PositionRotator(this);
        }

        protected override void OnEnemySpawn()
        {
            transform.position = SpaceUtil.WorldMap.Center;
        }

        protected override void OnEnemyFrame(float deltaTime)
        {
            Velocity = MathUtil.Vector2AtRadianAngle(TotalTime);
            Rotator.RunFrame(deltaTime);
        }
    }
}