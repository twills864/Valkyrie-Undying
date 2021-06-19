using System;
using System.Linq;
using Assets.Bullets.EnemyBullets;
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

        #region Prefabs

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

        #endregion Prefabs


        #region Prefab Properties

        private float TravelTime => _TravelTime;
        private float RotateTime => _RotateTime;
        private float FireTime => _FireTime;
        private Vector2 MinNewDistanceOffset => _MinNewDistanceOffset;
        private Vector2 MaxNewDistanceOffset => _MaxNewDistanceOffset;
        private int BulletsInBurst => _BulletsInBurst;
        private Vector2 WorldEdgeTolerance => _WorldEdgeTolerance;


        #endregion Prefab Properties


        protected override EnemyFireStrategy InitialFireStrategy()
            => new WaspEnemyStrategy();

        private WaspEnemyStrategy WaspFireStrategy { get; set; }

        public override Vector3 FirePosition => transform.position +
            MathUtil.Vector3AtDegreeAngle(RotationDegrees, SpriteMap.WidthHalf);

        private enum FrameBehaviors
        {
            FlyingToNewDestination0,
            RotatingToPlayer1,
            FiringBullets2,
            RotatingToNewDestination3
        }

        private FrameBehaviors _FrameBehavior;
        private FrameBehaviors FrameBehavior
        {
            get => _FrameBehavior;
            set
            {
                _FrameBehavior = value;

                switch (value)
                {
                    case (FrameBehaviors.FlyingToNewDestination0):
                        break;
                    case FrameBehaviors.RotatingToPlayer1:
                        RotateEase.ResetSelf();
                        Rotate.SetMinimumArcAngleRange(RotationDegrees, AngleDegreesToPlayer);
                        break;
                    case FrameBehaviors.FiringBullets2:
                        BulletsFired = 0;
                        FireTimer.Reset();
                        break;
                    case FrameBehaviors.RotatingToNewDestination3:
                        NextDestination = PickNewDestination();

                        RotateEase.ResetSelf();
                        float endAngle = MathUtil.AngleDegreesFromPoints(transform.position, NextDestination);
                        Rotate.SetMinimumArcAngleRange(RotationDegrees, endAngle);
                        break;
                    default:
                        throw ExceptionUtil.ArgumentException(() => FrameBehavior);
                }
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

        private int BulletsFired { get; set; }

        private RotateTo Rotate { get; set; }
        private EaseIn RotateEase { get; set; }
        private float AngleDegreesToPlayer => MathUtil.AngleDegreesFromPoints(transform.position, Player.Position);


        private LoopingFrameTimer FireTimer { get; set; }

        protected override void OnEnemyInit()
        {
            WaspFireStrategy = (WaspEnemyStrategy)FireStrategy;

            NewDestinationMove = MoveTo.Default(this, TravelTime);
            NewDestinationEase = new EaseIn(NewDestinationMove);

            Rotate = RotateTo.Default(this, RotateTime);
            RotateEase = new EaseIn(Rotate);

            FireTimer = new LoopingFrameTimer(FireTime);
        }

        protected override void OnEnemySpawn()
        {
            const float InitialYBoost = 1.0f;
            NextDestination = PickNewDestination().AddY(-InitialYBoost);
            RotationDegrees = MathUtil.AngleDegreesFromPoints(transform.position, NextDestination);
            FrameBehavior = FrameBehaviors.FlyingToNewDestination0;
        }

        protected override void OnEnemyFrame(float deltaTime, float realDeltaTime)
        {
            switch (FrameBehavior)
            {
                case (FrameBehaviors.FlyingToNewDestination0):
                    FlyToNewDestination(deltaTime);
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
                default:
                    throw ExceptionUtil.ArgumentException(() => FrameBehavior);
            }
        }

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

            if(!SpaceUtil.PointIsInBounds(newDestination))
            {

            }

            return newDestination;
        }

        private float CompensateNewDestinationOverflow(float position, float limit)
        {
            float overflow = position - limit;
            float newPosition = position - (overflow * 2f);
            return newPosition;
        }

        private void FlyToNewDestination(float deltaTime)
        {
            if (NewDestinationEase.FrameRunFinishes(deltaTime))
                FrameBehavior = FrameBehaviors.RotatingToPlayer1;
        }

        private void RotateToPlayer(float deltaTime)
        {
            float anglesToPlayer = AngleDegreesToPlayer;
            if (anglesToPlayer < 0f)
                anglesToPlayer += 360f;

            Rotate.SetMinimumArcAngleRange(Rotate.StartRotationDegrees, anglesToPlayer);

            if (RotateEase.FrameRunFinishes(deltaTime))
                FrameBehavior = FrameBehaviors.FiringBullets2;
        }

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

        private void RotateToNewDestination(float deltaTime)
        {
            if (RotateEase.FrameRunFinishes(deltaTime))
                FrameBehavior = FrameBehaviors.FlyingToNewDestination0;
        }
    }
}
