using Assets.Bullets.EnemyBullets;
using Assets.FireStrategies.EnemyFireStrategies;
using UnityEngine;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public class SwivelEnemy : Enemy
    {
        [SerializeField]
        private float SwivelAngle;

        [SerializeField]
        private float MinAngle;
        [SerializeField]
        private float MaxAngle;

        protected override EnemyFireStrategy InitialFireStrategy()
            => new SwivelEnemyFireStrategy();

        protected override void OnEnemyInit()
        {
            const float AngleDown = 270;
            RotationDegrees = AngleDown;

            MinAngle = AngleDown - SwivelAngle;
            MaxAngle = AngleDown + SwivelAngle;


        }
    }
}