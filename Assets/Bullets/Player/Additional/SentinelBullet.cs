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

        const float TravelTime = 2.0f;
        private FrameTimer TravelTimer { get; } = new FrameTimer(TravelTime);

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
    }
}