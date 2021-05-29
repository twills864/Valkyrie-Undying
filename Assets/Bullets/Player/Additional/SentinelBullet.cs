using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.Powerups;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class SentinelBullet : PlayerBullet
    {
        protected override bool ShouldDeactivateOnDestructor => false;
        protected override bool ShouldMarkSelfCollision => false;
        public override AudioClip FireSound => SoundBank.LaserGritty;

        #region Prefabs

        [SerializeField]
        private float _TravelTime = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private float TravelTime => _TravelTime;

        #endregion Prefab Properties

        /// <summary>
        /// The index within its holding ObjectPool.
        /// </summary>
        public int Index { get; set; }
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

        protected override void OnActivate()
        {
            TravelTimer.Reset();
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            TravelTimer.Increment(deltaTime);
        }

        public override void OnCollideWithEnemy(Enemy enemy)
        {
            PlayHitSound();
            SentinelManager.Instance.SentinelTriggered(this, enemy);
            DeactivateSelf();
        }
    }
}