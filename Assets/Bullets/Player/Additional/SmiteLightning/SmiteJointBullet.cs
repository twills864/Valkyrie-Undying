using System;
using System.Linq;
using Assets.Util;
using Assets.Constants;
using Assets.GameTasks;
using Assets.ObjectPooling;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    ///
    /// </summary>
    /// <inheritdoc/>
    public class SmiteJointBullet : SmiteBullet
    {
        #region Prefabs

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

        #endregion Prefab Properties

        private float MaxWidthSq => MaxWidth * MaxWidth;

        private FrameTimer NextBranchTimer { get; set; }

        public override float Scale
        {
            get => LocalScaleX;
            set => transform.localScale = new Vector3(value, value, 1f);
        }

        protected override void OnPlayerBulletInit()
        {
            NextBranchTimer = new FrameTimer(TimeUntilNextSpawn);
            SetFadeOutSequence(FadeOutTime);
        }

        protected override void OnActivate()
        {
            NextBranchTimer.Reset();
        }

        //public override void OnSpawn()
        //{

        //}

        protected sealed override void OnSmiteBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            if (!NextBranchTimer.Activated)
            {
                if (NextBranchTimer.UpdateActivates(deltaTime))
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

            if(lightning.FadeOutSequence == null)
                lightning.SetFadeOutSequence(FadeOutTime);

            lightning.OnSpawn();

            var joint = PoolManager.Instance.BulletPool.Get<SmiteJointBullet>(nextSpawn.NextJointPoint);
            joint.InitFromPreviousLink(lightning);
            joint.OnSpawn();

            if (!nextSpawn.ReachedDestination)
            {
                joint.OnSpawn();
            }
            else
            {
                joint.NextBranchTimer.ActivateSelf();

                SmiteBullet link = joint;

                while (link != null)
                {
                    link.BeginDeactivating();
                    //DebugUtil.RedX(link.transform.position);
                    link = link.Previous;
                }
            }

            return joint;
        }

        private struct NextSpawnDetails
        {
            public float Angle;
            public float Width;
            public Vector3 SpawnPoint;
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
            if(distSq > MaxWidthSq)
            {
                details.ReachedDestination = false;

                float delta = RandomUtil.Float(-MaxAngleDelta, MaxAngleDelta);
                details.Angle += delta;

                details.Width = RandomUtil.Float(MinWidth, MaxWidth);

                Vector3 distanceDelta = MathUtil.Vector3AtDegreeAngle(details.Angle, details.Width);
                details.NextJointPoint = transform.position + distanceDelta;

                details.SpawnPoint = transform.position + (0.5f * distanceDelta);
            }
            else
            {
                details.ReachedDestination = true;

                details.Width = Mathf.Sqrt(distSq);

                details.NextJointPoint = TargetPosition;
                details.SpawnPoint = 0.5f * (transform.position + details.NextJointPoint);
            }

            return details;
        }


        public static void StartSmite(Vector3 startPosition, Vector3 targetPosition, int damage)
        {
            var bullet = PoolManager.Instance.BulletPool.Get<SmiteJointBullet>(startPosition);

            bullet.Previous = null;
            bullet.Head = bullet;

            bullet.TargetPosition = targetPosition;
            bullet.SmiteDamage = damage;

            bullet.NextBranchTimer.ActivateSelf();
            bullet.SpawnNextBranch();

            bullet.OnSpawn();
        }
    }
}