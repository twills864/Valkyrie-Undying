using Assets.FireStrategies.EnemyFireStrategies;
using Assets.Util;
using UnityEngine;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public class RingEnemyRing : PermanentVelocityEnemy
    {
        private const float HealthbarFadeTime = 0.5f;
        public override bool CanVoidPause => false;

        public override int BaseSpawnHealth => 125;
        public override float SpawnHealthScaleRate => 1f;

        public override EnemyFireStrategy FireStrategy { get; protected set; }
            = new InactiveEnemyStrategy();

        public float Height { get; private set; }
        public float HeightHalf => Height * 0.5f;
        private FrameTimer AlphaCalculator = new FrameTimer(HealthbarFadeTime);

        public RingEnemy Host { get; set; }

        protected override void OnEnemyInit()
        {
            Height = Sprite.bounds.size.y;
        }

        protected override void OnEnemySpawn()
        {
            //Debug.Break();
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

        protected override void OnEnemyDeactivate()
        {
            if (Host != null)
            {
                Host.Ring = null;
                Host = null;
            }
        }
    }
}