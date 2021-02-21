using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class SentinelBullet : PlayerBullet
    {
        protected override bool ShouldMarkSelfCollision => false;

        [SerializeField]
        private float TravelTime;
        private FrameTimer TravelTimer { get; set; }

        protected override void OnPlayerBulletInit()
        {
            TravelTimer = new FrameTimer(TravelTime);
        }

        public float DistanceRatio => TravelTimer.RatioComplete;
        protected override void OnPlayerBulletFrameRun(float deltaTime)
        {
            if (!TravelTimer.Activated)
                TravelTimer.Increment(deltaTime);
        }

        protected override void OnActivate()
        {
            TravelTimer.Reset();
        }

        public void UpdateTimerRatio(float newRatio)
        {
            TravelTimer.RatioComplete *= newRatio;
        }
    }
}