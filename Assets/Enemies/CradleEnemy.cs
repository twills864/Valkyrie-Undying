using System;
using Assets.Bullets.EnemyBullets;
using Assets.Constants;
using Assets.FireStrategies.EnemyFireStrategies;
using Assets.GameTasks;
using Assets.Util;
using UnityEngine;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public class CradleEnemy : Enemy
    {
        private const float AngleDown = 270;

        [SerializeField]
        private float SwivelAngle = GameConstants.PrefabNumber;

        [SerializeField]
        private float InitialTravelTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float FinalTravelSpeed = GameConstants.PrefabNumber;

        [SerializeField]
        private float RotateTime = GameConstants.PrefabNumber;

        private float MinAngle;
        private float MaxAngle;

        private Vector2 FireLeftVelocity;
        private Vector2 FireRightVelocity => new Vector2(-FireLeftVelocity.x, FireLeftVelocity.y);

        private Vector2 FireLeftOffset;
        private Vector2 FireRightOffset => new Vector2(-FireLeftOffset.x, FireLeftOffset.y);

        private Vector3 FirePositionLeft => FirePosition + (Vector3)FireLeftOffset;
        private Vector3 FirePositionRight => FirePosition + (Vector3)FireRightOffset;

        protected override EnemyFireStrategy InitialFireStrategy()
            => new CradleEnemyFireStrategy();

        private CradleEnemyFireStrategy CradleEnemyFireStrategy
            => (CradleEnemyFireStrategy)FireStrategy;

        private EndlessSequence Behavior { get; set; }

        #region Init

        protected override void OnEnemyInit()
        {
            InitAngles();
            InitSequences();
        }

        private void InitAngles()
        {
            RotationDegrees = AngleDown;

            MinAngle = AngleDown - SwivelAngle;
            MaxAngle = AngleDown + SwivelAngle;

            FireLeftVelocity = MathUtil.Vector2AtDegreeAngle(MinAngle);

            Vector3 rightPosition = new Vector3(SpriteMap.WidthHalf, 0);
            Vector3 minAngleEulerDegrees = new Vector3(0, 0, MinAngle);
            Quaternion minAngleQuaternion = Quaternion.Euler(minAngleEulerDegrees);
            FireLeftOffset = minAngleQuaternion * rightPosition;
            FireLeftOffset.y += SpriteMap.WidthHalf;
        }

        private void InitSequences()
        {
            var worldMap = SpaceUtil.WorldMap;
            float startYPosition = worldMap.Top.y + SpriteMap.HeightHalf;
            float targetYPosition = (worldMap.Center.y + (worldMap.Top.y * 2f)) * 0.333f;

            float yDiff = targetYPosition - startYPosition;
            Vector3 moveDistance = new Vector3(0, yDiff, 0);

            float rotateTimeHalf = RotateTime * 0.5f;

            // Initial sequence
            var moveDown = new MoveBy(this, moveDistance, InitialTravelTime);
            var setVelocity = new GameTaskFunc(this, () => Velocity = new Vector2(0, -FinalTravelSpeed));
            var initialRotateLeft = new RotateTo(this, AngleDown, MinAngle, rotateTimeHalf);

            var initialSequence = new Sequence(moveDown, setVelocity, initialRotateLeft);

            // Infinite sequence
            var fireLeft = new GameTaskFunc(this, FireLeft);
            var rotateMiddle1 = new RotateTo(this, MinAngle, AngleDown, rotateTimeHalf);
            var fireDown = new GameTaskFunc(this, FireDown);
            var rotateRight = new RotateTo(this, AngleDown, MaxAngle, rotateTimeHalf);
            var fireRight = new GameTaskFunc(this, FireRight);
            var rotateMiddle2 = new RotateTo(this, MaxAngle, AngleDown, rotateTimeHalf);
            var rotateLeft = new RotateTo(this, AngleDown, MinAngle, rotateTimeHalf);

            var infiniteSequence = new Sequence(fireLeft, rotateMiddle1, fireDown,
                rotateRight, fireRight, rotateMiddle2, fireDown, rotateLeft);

            var repeat = new RepeatForever(infiniteSequence);
            Behavior = new EndlessSequence(repeat, initialSequence);
        }

        #endregion Init

        protected override void OnEnemySpawn()
        {
            RotationDegrees = AngleDown;
            Behavior.ResetSelf();
        }

        protected override void OnEnemyFrame(float deltaTime)
        {
            Behavior.RunFrame(deltaTime);
        }

        private void FireDown()
        {
            CradleEnemyFireStrategy.GetBullets(FirePosition, Vector2.down);
        }
        private void FireLeft()
        {
            CradleEnemyFireStrategy.GetBullets(FirePositionLeft, FireLeftVelocity);
        }

        private void FireRight()
        {
            CradleEnemyFireStrategy.GetBullets(FirePositionRight, FireRightVelocity);
        }
    }
}