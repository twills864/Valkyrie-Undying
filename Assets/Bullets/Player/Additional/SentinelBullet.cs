﻿using Assets.Bullets.PlayerBullets;
using Assets.Components;
using Assets.Constants;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.Powerups;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// The bullet spawned after the player collects the Sentinel powerup.
    /// This bullet will orbit the player, and is managed by the SentinelManager class.
    /// </summary>
    /// <inheritdoc/>
    public class SentinelBullet : PlayerBullet
    {
        protected override bool ShouldDeactivateOnDestructor => false;
        protected override bool ShouldMarkSelfCollision => false;
        public override AudioClip FireSound => SoundBank.LaserGritty;

        // SentinelManager.Instance.CalculateRepresentedVelocity(this);
        public override Vector2 RepresentedVelocity => RepresentedVelocityTracker.RepresentedVelocity;

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

        private RepresentedVelocityTracker RepresentedVelocityTracker { get; set; }

        protected override void OnPlayerBulletInit()
        {
            TravelTimer = new EaseInTimer(TravelTime);
            RepresentedVelocityTracker = new RepresentedVelocityTracker(this);
        }

        protected override void OnActivate()
        {
            TravelTimer.Reset();
        }

        protected override void OnBulletSpawn()
        {
            RepresentedVelocityTracker.OnSpawn();
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            TravelTimer.Increment(deltaTime);
            RepresentedVelocityTracker.RunFrame(deltaTime, realDeltaTime);
        }

        protected override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            SentinelManager.Instance.SentinelTriggered(this, enemy);
        }

        public void RepresentedVelocityTrackerLateUpdate()
        {
            RepresentedVelocityTracker.OnLateUpdate();
        }
    }
}