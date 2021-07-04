using Assets.FireStrategies.EnemyFireStrategies;
using Assets.Util;
using UnityEngine;

namespace Assets.Enemies
{
    /// <summary>
    /// The ring that surrounds and protects the Ring enemy.
    /// </summary>
    /// <inheritdoc/>
    public class RingEnemyRing : PermanentVelocityEnemy
    {
        private const float HealthbarFadeTime = 0.5f;
        public override bool CanVoidPause => false;
        public override bool CanChill => false;
        public override bool InfluencesDirectorGameBalance => false;
        public override int ActiveTrackedEnemiesCountContribution => 0;
        public override AudioClip FireSound => SoundBank.Silence;

        public float Height { get; private set; }
        public float HeightHalf => Height * 0.5f;
        private FrameTimer AlphaCalculator = new FrameTimer(HealthbarFadeTime);

        public RingEnemy Host { get; set; }

        protected override EnemyFireStrategy InitialFireStrategy()
            => new InactiveEnemyStrategy();

        protected override void OnPermanentVelocityEnemyInit()
        {
            Height = Sprite.bounds.size.y;
        }

        protected override void OnEnemySpawn()
        {
            //Debug.Break();
            AlphaCalculator.Reset();
            HealthBar.Alpha = 0f;
        }

        protected override void OnFireStrategyEnemyFrame(float deltaTime, float realDeltaTime)
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