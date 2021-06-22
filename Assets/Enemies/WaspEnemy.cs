using System;
using System.Linq;
using Assets.Bullets.EnemyBullets;
using Assets.Components;
using Assets.Constants;
using Assets.FireStrategies.EnemyFireStrategies;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.Enemies
{
    /// <summary>
    /// An enemy that flies to a random spot on the top half of the screen,
    /// fires bullets directly at the player, then repeats the process
    /// with a new random spot.
    /// </summary>
    /// <inheritdoc/>
    public class WaspEnemy : Enemy
    {
        public override AudioClip FireSound => SoundBank.GunPistol;

        #region Property Fields
        private FrameBehaviors _FrameBehavior;
        #endregion Property Fields

        #region Prefabs

        [SerializeField]
        private float _InitialTravelSpeed = GameConstants.PrefabNumber;

        [SerializeField]
        private float _TravelTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _RotateTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _FireTime = GameConstants.PrefabNumber;

        [SerializeField]
        private Vector2 _MinNewDistanceOffset = Vector2.zero;

        [SerializeField]
        private Vector2 _MaxNewDistanceOffset = Vector2.zero;

        [SerializeField]
        private Vector2 _WorldEdgeTolerance = Vector2.zero;

        [SerializeField]
        private int _BulletsInBurst = GameConstants.PrefabNumber;

        [SerializeField]
        private float _FireBehindSpreadAngle = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private float InitialTravelSpeed => _InitialTravelSpeed;
        private float TravelTime => _TravelTime;
        private float RotateTime => _RotateTime;
        private float FireTime => _FireTime;
        private Vector2 MinNewDistanceOffset => _MinNewDistanceOffset;
        private Vector2 MaxNewDistanceOffset => _MaxNewDistanceOffset;
        private int BulletsInBurst => _BulletsInBurst;
        private Vector2 WorldEdgeTolerance => _WorldEdgeTolerance;
        private float FireBehindSpreadAngle => _FireBehindSpreadAngle;

        #endregion Prefab Properties


        protected override EnemyFireStrategy InitialFireStrategy()
            => new WaspEnemyStrategy();

        public override Vector2 RepresentedVelocity => RepresentedVelocityTracker.RepresentedVelocity;

        private WaspEnemyStrategy WaspFireStrategy { get; set; }

        public override Vector3 FirePosition => transform.position +
            MathUtil.Vector3AtDegreeAngle(RotationDegrees, SpriteMap.WidthHalf);

        private Vector3 BehindFirePosition =>  transform.position
            + MathUtil.Vector3AtDegreeAngle((RotationDegrees + 180f), SpriteMap.Width * 0.25f);

        private enum FrameBehaviors
        {
            InitialFlyIn0,
            RotatingToPlayer1,
            FiringBullets2,
            RotatingToNewDestination3,
            FlyingToNewDestination4,
            VoidResumedRotateToPlayer,
        }

        private FrameBehaviors FrameBehavior
        {
            get => _FrameBehavior;
            set
            {
                _FrameBehavior = value;

                switch (value)
                {
                    case (FrameBehaviors.InitialFlyIn0):
                        break;
                    case FrameBehaviors.RotatingToPlayer1:
                        RotateEase.ResetSelf();
                        Rotate.SetMinimumArcAngleRange(RotationDegrees, AngleDegreesToPlayer);

                        // Reset BulletsFired in this step so that going from
                        // VoidResumedRotateToPlayer to FiringBullets2
                        // doesn't reset the bullet count.
                        BulletsFired = 0;
                        break;
                    case FrameBehaviors.FiringBullets2:
                        FireTimer.Reset();
                        break;
                    case FrameBehaviors.RotatingToNewDestination3:
                        NextDestination = PickNewDestination();

                        RotateEase.ResetSelf();
                        float endAngle = MathUtil.AngleDegreesFromPoints(transform.position, NextDestination);
                        Rotate.SetMinimumArcAngleRange(RotationDegrees, endAngle);
                        break;
                    case FrameBehaviors.FlyingToNewDestination4:
                        FireBehind();
                        break;
                    case FrameBehaviors.VoidResumedRotateToPlayer:
                        VoidResumeRotateEase.ResetSelf();
                        VoidResumeRotate.SetMinimumArcAngleRange(RotationDegrees, AngleDegreesToPlayer);
                        break;
                    default:
                        throw ExceptionUtil.ArgumentException(() => FrameBehavior);
                }
            }
        }

        private Vector3 InitialDestination
        {
            get => InitialDestinationMove.Destination;
            set
            {
                Vector3 newDistance = value - transform.position;

                // time = distance / speed
                const float FlatAddition = 0.25f;
                InitialDestinationEase.Duration = (newDistance.magnitude / InitialTravelSpeed) + FlatAddition;

                InitialDestinationEase.ResetSelf();
                InitialDestinationMove.ReinitializeMove(transform.position, value);
            }
        }

        private Vector3 NextDestination
        {
            get => NewDestinationMove.Destination;
            set
            {
                NewDestinationEase.ResetSelf();
                NewDestinationMove.ReinitializeMove(transform.position, value);
            }
        }

        private MoveTo NewDestinationMove { get; set; }
        private EaseIn NewDestinationEase { get; set; }

        private MoveTo InitialDestinationMove { get; set; }
        private EaseIn InitialDestinationEase { get; set; }

        private int BulletsFired { get; set; }

        private RotateTo Rotate { get; set; }
        private EaseIn RotateEase { get; set; }
        private float AngleDegreesToPlayer => MathUtil.AngleDegreesFromPoints(transform.position, Player.Position);

        private RotateTo VoidResumeRotate { get; set; }
        private EaseIn VoidResumeRotateEase { get; set; }


        private LoopingFrameTimer FireTimer { get; set; }

        private RepresentedVelocityTracker RepresentedVelocityTracker { get; set; }

        protected override void OnEnemyInit()
        {
            WaspFireStrategy = (WaspEnemyStrategy)FireStrategy;

            InitialDestinationMove = MoveTo.Default(this, 1.0f);
            InitialDestinationEase = new EaseIn(InitialDestinationMove);

            NewDestinationMove = MoveTo.Default(this, TravelTime);
            NewDestinationEase = new EaseIn(NewDestinationMove);

            Rotate = RotateTo.Default(this, RotateTime);
            RotateEase = new EaseIn(Rotate);

            VoidResumeRotate = RotateTo.Default(this, RotateTime * 0.5f);
            VoidResumeRotateEase = new EaseIn(VoidResumeRotate);

            FireTimer = new LoopingFrameTimer(FireTime);

            RepresentedVelocityTracker = new RepresentedVelocityTracker(this);
        }

        protected override void OnEnemySpawn()
        {
            const float InitialYBoost = 1.0f;
            InitialDestination = PickNewDestination().AddY(-InitialYBoost);
            RotationDegrees = MathUtil.AngleDegreesFromPoints(transform.position, InitialDestination);

            FrameBehavior = FrameBehaviors.InitialFlyIn0;

            RepresentedVelocityTracker.OnSpawn();
        }

        protected override void OnEnemyFrame(float deltaTime, float realDeltaTime)
        {
            switch (FrameBehavior)
            {
                case (FrameBehaviors.InitialFlyIn0):
                    FlyToInitialDestination(deltaTime);
                    break;
                case FrameBehaviors.RotatingToPlayer1:
                    RotateToPlayer(deltaTime);
                    break;
                case FrameBehaviors.FiringBullets2:
                    FireBullets(deltaTime);
                    break;
                case FrameBehaviors.RotatingToNewDestination3:
                    RotateToNewDestination(deltaTime);
                    break;
                case FrameBehaviors.FlyingToNewDestination4:
                    FlyToNewDestination(deltaTime);
                    break;
                case FrameBehaviors.VoidResumedRotateToPlayer:
                    VoidResumeRotateToPlayer(deltaTime);
                    break;
                default:
                    throw ExceptionUtil.ArgumentException(() => FrameBehavior);
            }

            RepresentedVelocityTracker.RunFrame(deltaTime, realDeltaTime);
        }

        #region PickNewDestination

        private Vector3 PickNewDestination()
        {
            Vector3 offset = RandomUtil.Vector2(MinNewDistanceOffset, MaxNewDistanceOffset);
            offset.x *= RandomUtil.RandomlyNegative(1f);
            offset.y *= RandomUtil.RandomlyNegative(1f);

            Vector3 newDestination = transform.position + offset;

            if(offset.x < 0)
            {
                float minX = SpaceUtil.WorldMap.Left.x + WorldEdgeTolerance.x;
                if (newDestination.x < minX)
                    newDestination.x = CompensateNewDestinationOverflow(newDestination.x, minX);
            }
            else
            {
                float maxX = SpaceUtil.WorldMap.Right.x - WorldEdgeTolerance.x;
                if (newDestination.x > maxX)
                    newDestination.x = CompensateNewDestinationOverflow(newDestination.x, maxX);
            }

            if (offset.y < 0)
            {
                float minY = SpaceUtil.WorldMap.Center.y;
                if (newDestination.y < minY)
                    newDestination.y = CompensateNewDestinationOverflow(newDestination.y, minY);
            }
            else
            {
                float maxY = SpaceUtil.WorldMap.Top.y - WorldEdgeTolerance.y;
                if (newDestination.y > maxY)
                    newDestination.y = CompensateNewDestinationOverflow(newDestination.y, maxY);
            }

            return newDestination;
        }

        private float CompensateNewDestinationOverflow(float position, float limit)
        {
            float overflow = position - limit;
            float newPosition = position - (overflow * 2f);
            return newPosition;
        }

        #endregion PickNewDestination

        #region FrameBehaviors.InitialFlyIn0

        private void FlyToInitialDestination(float deltaTime)
        {
            if (InitialDestinationEase.FrameRunFinishes(deltaTime))
                FrameBehavior = FrameBehaviors.RotatingToPlayer1;
        }

        #endregion FrameBehaviors.InitialFlyIn0

        #region FrameBehaviors.RotatingToPlayer1

        private void RotateToPlayer(float deltaTime)
        {
            float anglesToPlayer = AngleDegreesToPlayer;
            if (anglesToPlayer < 0f)
                anglesToPlayer += 360f;

            Rotate.SetMinimumArcAngleRange(Rotate.StartRotationDegrees, anglesToPlayer);

            if (RotateEase.FrameRunFinishes(deltaTime))
                FrameBehavior = FrameBehaviors.FiringBullets2;
        }

        #endregion FrameBehaviors.RotatingToPlayer1

        #region FrameBehaviors.FiringBullets2

        private void FireBullets(float deltaTime)
        {
            float degrees = AngleDegreesToPlayer;
            RotationDegrees = degrees;

            if (FireTimer.UpdateActivates(deltaTime))
            {
                if (BulletsFired != BulletsInBurst)
                {
                    WaspFireStrategy.GetBullets(FirePosition, degrees);
                    BulletsFired++;
                }
                else
                {
                    FrameBehavior = FrameBehaviors.RotatingToNewDestination3;
                }
            }
        }

        #endregion FrameBehaviors.FiringBullets2

        #region FrameBehaviors.RotatingToNewDestination3

        private void RotateToNewDestination(float deltaTime)
        {
            if (RotateEase.FrameRunFinishes(deltaTime))
                FrameBehavior = FrameBehaviors.FlyingToNewDestination4;
        }

        #endregion

        #region FrameBehaviors.FlyingToNewDestination4

        private void FireBehind()
        {
            WaspFireStrategy.FireBehind(BehindFirePosition, RotationDegrees, FireBehindSpreadAngle);
        }

        private void FlyToNewDestination(float deltaTime)
        {
            if (NewDestinationEase.FrameRunFinishes(deltaTime))
                FrameBehavior = FrameBehaviors.RotatingToPlayer1;
        }

        #endregion FrameBehaviors.FlyingToNewDestination4

        #region FrameBehaviors.VoidResumedRotateToPlayer

        protected override void OnEnemyVoidResume()
        {
            // Reset the rotation in the FrameBehavior setter
            if (FrameBehavior == FrameBehaviors.RotatingToPlayer1)
                FrameBehavior = FrameBehaviors.RotatingToPlayer1;
            else if (FrameBehavior == FrameBehaviors.FiringBullets2)
                FrameBehavior = FrameBehaviors.VoidResumedRotateToPlayer;
        }

        private void VoidResumeRotateToPlayer(float deltaTime)
        {
            float anglesToPlayer = AngleDegreesToPlayer;
            if (anglesToPlayer < 0f)
                anglesToPlayer += 360f;

            VoidResumeRotate.SetMinimumArcAngleRange(VoidResumeRotate.StartRotationDegrees, anglesToPlayer);

            if (VoidResumeRotateEase.FrameRunFinishes(deltaTime))
                FrameBehavior = FrameBehaviors.FiringBullets2;
        }

        #endregion FrameBehaviors.VoidResumedRotateToPlayer

        private void LateUpdate()
        {
            RepresentedVelocityTracker.OnLateUpdate();
        }

    }
}
