using Assets.FireStrategies.EnemyFireStrategies;
using Assets.Util;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public class RingEnemyRing : PermanentVelocityEnemy
    {
        private const float HealthbarFadeTime = 0.5f;

        public override int BaseSpawnHealth => 125;
        public override float SpawnHealthScaleRate => 1f;

        public override EnemyFireStrategy FireStrategy { get; protected set; }
            = new InactiveEnemyStrategy();

        private FrameTimer AlphaCalculator = new FrameTimer(HealthbarFadeTime);

        public RingEnemy Host { get; set; }

        protected override void OnEnemyActivate()
        {
            AlphaCalculator.Reset();
            HealthBar.Alpha = 0f;
        }

        protected override void OnEnemyFrame(float deltaTime)
        {
            if(!AlphaCalculator.Activated)
            {
                AlphaCalculator.Increment(deltaTime);
                HealthBar.Alpha = AlphaCalculator.RatioComplete;
            }
        }

        protected override void OnDeactivate()
        {
            if (Host != null)
            {
                Host.Ring = null;
                Host = null;
            }
        }
    }
}