using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class SentinelBullet : PlayerBullet
    {
        protected override bool ShouldDeactivateOnDestructor => false;
        protected override bool ShouldMarkSelfCollision => false;

        [SerializeField]
        private float TravelTime;
        private EaseInTimer TravelTimer { get; set; }

        /// <summary>
        /// The final radius this Sentinel will hover around the player.
        /// </summary>
        public float MaxRadius { get; set; }

        /// <summary>
        /// The distance representing how far this Sentinel has traveled to MaxRadius.
        /// </summary>
        public float CurrentRadius
        {
            get => MaxRadius * RadiusRatio;
        }

        /// <summary>
        /// The ratio representing how far this Sentinel has traveled to MaxRadius.
        /// </summary>
        public float RadiusRatio
        {
            get => TravelTimer.RatioComplete;
            set => TravelTimer.RatioComplete = value;
        }

        protected override void OnPlayerBulletInit()
        {
            TravelTimer = new EaseInTimer(TravelTime);
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime)
        {
            TravelTimer.Increment(deltaTime);
        }

        protected override void OnActivate()
        {
            TravelTimer.Reset();
        }
    }
}