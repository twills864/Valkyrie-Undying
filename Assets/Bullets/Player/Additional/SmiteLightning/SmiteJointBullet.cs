using System;
using System.Linq;
using Assets.Util;
using Assets.Constants;
using Assets.GameTasks;
using Assets.ObjectPooling;
using UnityEngine;
using System.Collections.Generic;
using Assets.Enemies;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// Represents the joint between two lines of a Smite lightning bolt.
    /// While it contains a bulk of the logic that makes the Smite powerup work,
    /// it currently has no visible render element.
    /// </summary>
    /// <inheritdoc/>
    public class SmiteJointBullet : SmiteBullet
    {
        public static void StartSmite(Vector3 startPosition, Enemy target)
        {
            var bullet = PoolManager.Instance.BulletPool.Get<SmiteJointBullet>(startPosition);
            bullet.InitWithTarget(target);
            bullet.PlayFireSound();
        }

        #region Prefabs
        [SerializeField]
        private int _DamageDealtToTarget = GameConstants.PrefabNumber;

        [SerializeField]
        private float _TimeUntilNextSpawn = GameConstants.PrefabNumber;

        [SerializeField]
        private float _MinWidth = GameConstants.PrefabNumber;

        [SerializeField]
        private float _MaxWidth = GameConstants.PrefabNumber;

        [SerializeField]
        private float _MaxAngleDelta = GameConstants.PrefabNumber;

        [SerializeField]
        private float _FadeOutTime = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private float TimeUntilNextSpawn => _TimeUntilNextSpawn;
        private float MinWidth => _MinWidth;
        private float MaxWidth => _MaxWidth;
        private float MaxAngleDelta => _MaxAngleDelta;
        public float FadeOutTime => _FadeOutTime;
        private int DamageDealtToTarget => _DamageDealtToTarget;

        #endregion Prefab Properties

        private float MaxWidthSq => MaxWidth * MaxWidth;

        private FrameTimer NextBranchTimer { get; set; }

        public override float Scale
        {
            get => LocalScaleX;
            set => transform.localScale = new Vector3(value, value, 1f);
        }

        protected override void OnSmiteBulletInit()
        {
            NextBranchTimer = new FrameTimer(TimeUntilNextSpawn);
            SetFadeOutSequenceFadeTime(FadeOutTime);
        }

        protected override void OnActivate()
        {
            NextBranchTimer.Reset();
        }

        protected sealed override void OnSmiteBulletFrameRun(float realDeltaTime)
        {
            if (!NextBranchTimer.Activated)
            {
                if (NextBranchTimer.UpdateActivates(realDeltaTime))
                    SpawnNextBranch(NextBranchTimer.OverflowDeltaTime);
            }
        }

        private void SpawnNextBranch(float overflowDeltaTime)
        {
            var bullet = SpawnNextBranch();
            bullet.RunFrame(overflowDeltaTime, overflowDeltaTime);
        }

        private SmiteJointBullet SpawnNextBranch()
        {
            NextSpawnDetails nextSpawn = GetNextSpawnDetails();

            var lightning = PoolManager.Instance.BulletPool.Get<SmiteLightningBullet>(nextSpawn.SpawnPoint);

            lightning.RotationDegrees = nextSpawn.Angle;
            lightning.Scale = nextSpawn.Width;

            lightning.InitFromPreviousLink(this);

            lightning.SetFadeOutSequenceFadeTime(FadeOutTime);

            lightning.SetRepresentedVelocity(nextSpawn.DistanceDelta);

            lightning.OnSpawn();

            var joint = PoolManager.Instance.BulletPool.Get<SmiteJointBullet>(nextSpawn.NextJointPoint);
            joint.InitFromPreviousLink(lightning);
            joint.SetRepresentedVelocity(nextSpawn.DistanceDelta);
            joint.OnSpawn();

            if (!nextSpawn.ReachedDestination)
                joint.OnSpawn();
            else
            {
                joint.NextBranchTimer.ActivateSelf();
                joint.DeactivateAllLinks();

                joint.PlayAllHitSounds();
            }

            return joint;
        }

        private struct NextSpawnDetails
        {
            public float Angle;
            public float Width;
            public Vector3 SpawnPoint;
            public Vector3 DistanceDelta;
            public Vector3 NextJointPoint;
            public bool ReachedDestination;
        }

        private NextSpawnDetails GetNextSpawnDetails()
        {
            NextSpawnDetails details = new NextSpawnDetails
            {
                Angle = MathUtil.AngleDegreesFromPoints(transform.position, TargetPosition)
            };

            float distSq = MathUtil.Vector3_2DDistanceSquared(transform.position, TargetPosition);
            bool targetOutOfReach = distSq > MaxWidthSq;
            if (targetOutOfReach)
            {
                details.ReachedDestination = false;

                float delta = RandomUtil.Float(-MaxAngleDelta, MaxAngleDelta);
                details.Angle += delta;

                details.Width = RandomUtil.Float(MinWidth, MaxWidth);

                details.DistanceDelta = MathUtil.Vector3AtDegreeAngle(details.Angle, details.Width);
                details.NextJointPoint = transform.position + details.DistanceDelta;

                details.SpawnPoint = transform.position + (0.5f * details.DistanceDelta);
            }
            else
            {
                details.ReachedDestination = true;

                details.Width = Mathf.Sqrt(distSq);

                details.NextJointPoint = TargetPosition;

                details.DistanceDelta = (details.NextJointPoint - transform.position);
                details.SpawnPoint = 0.5f * (transform.position + details.NextJointPoint);
            }

            return details;
        }

        private void InitWithTarget(Enemy target)
        {
            Previous = null;
            Head = this;

            TargetEnemy.Target = target;
            TargetPosition = target.transform.position;

            DamageOnHitAny = BaseDamage;
            DamageOnHitTarget = DamageDealtToTarget;

            NextBranchTimer.ActivateSelf();
            SpawnNextBranch();

            OnSpawn();
        }
    }
}