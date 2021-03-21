using System;
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
    /// <inheritdoc/>
    public class LaserEnemy : Enemy
    {
        private enum FrameBehaviors
        {
            RunFlyInFrame0,
            RunFireFrame1,
            WaitForLaserSpawner2,
            WaitForLaser3
        }

        [SerializeField]
        private float FlyTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float RotateTime = GameConstants.PrefabNumber;


        //[SerializeField]
        //private float LaserFadeInTime = GameConstants.PrefabNumber;

        //[SerializeField]
        //private float LaserFullBrightTime = GameConstants.PrefabNumber;

        //[SerializeField]
        //private float LaserFullBright = GameConstants.PrefabNumber

        private PositionRotator Rotator { get; set; }

        private LaserEnemyFireStrategy LaserEnemyFireStrategy
            => (LaserEnemyFireStrategy)FireStrategy;
        protected override EnemyFireStrategy InitialFireStrategy()
            => new LaserEnemyFireStrategy();

        private float MoveX
        {
            get => MoveHorizontal.DistanceX;
            set => MoveHorizontal.DistanceX = value;
        }

        private float MoveY
        {
            get => MoveVertical.DistanceY;
            set => MoveVertical.DistanceY = value;
        }

        private MoveBy MoveVertical { get; set; }
        private MoveBy MoveHorizontal { get; set; }
        private EaseIn FlyIn { get; set; }

        private RotateTo Rotate { get; set; }
        private Sequence FireSequence { get; set; }

        private FrameBehaviors FrameBehavior;
        //private FrameBehaviorArray FrameBehaviors;

        public float WidthHalf { get; private set; }

        private LaserEnemyBulletSpawner CurrentSpawner { get; set; }
        private LaserEnemyBullet CurrentLaser { get; set; }

        protected override void OnEnemyInit()
        {
            var sprite = GetComponent<SpriteRenderer>();
            WidthHalf = sprite.size.x * 0.5f;

            Rotator = new PositionRotator(this);

            #region FlyIn

            MoveVertical = new MoveBy(this, Vector3.zero, FlyTime);
            MoveHorizontal = new MoveBy(this, Vector3.zero, FlyTime);

            var easeVertical = new EaseIn(MoveVertical);
            var easeHorizontal = new EaseOut(MoveHorizontal);

            var concurrence = new ConcurrentGameTask(this, easeVertical, easeHorizontal);

            FlyIn = new EaseIn(concurrence);

            #endregion FlyIn

            #region FireLoop

            var resetRotate = new GameTaskFunc(this, ResetRotate);
            Rotate = new RotateTo(this, 0.0f, RotateTime);
            var fireLaser = new GameTaskFunc(this, FireLaser);
            FireSequence = new Sequence(resetRotate, Rotate, fireLaser);

            #endregion FireLoop
        }

        protected override void OnEnemySpawn()
        {
            Vector3 destination = SpaceUtil.RandomEnemyDestinationTopHalf(this);
            Vector3 difference = destination - transform.position;

            MoveX = difference.x;
            MoveY = difference.y;

            FrameBehavior = FrameBehaviors.RunFlyInFrame0;
            FlyIn.ResetSelf();
            FireSequence.ResetSelf();
        }

        protected override void OnEnemyFrame(float deltaTime)
        {

            switch(FrameBehavior)
            {
                case (FrameBehaviors.RunFlyInFrame0):
                    RunFlyInFrame0(deltaTime);
                    break;
                case FrameBehaviors.RunFireFrame1:
                    RunFireFrame1(deltaTime);
                    break;
                case FrameBehaviors.WaitForLaserSpawner2:
                    WaitForLaserSpawner2(deltaTime);
                    break;
                case FrameBehaviors.WaitForLaser3:
                    WaitForLaser3(deltaTime);
                    break;
                default:
                    throw ExceptionUtil.ArgumentException(() => FrameBehavior);
            }
        }

        private void RunFlyInFrame0(float deltaTime)
        {
            Rotator.RunFrame(deltaTime);
            if (FlyIn.FrameRunFinishes(deltaTime))
            {
                FrameBehavior = FrameBehaviors.RunFireFrame1;
                RunFireFrame1(FlyIn.OverflowDeltaTime);
            }
        }
        private void RunFireFrame1(float deltaTime)
        {
            if (FireSequence.FrameRunFinishes(deltaTime))
            {
                FrameBehavior = FrameBehaviors.WaitForLaserSpawner2;
                //WaitForLaser(FlyInSequence.OverflowDeltaTime);
            }
        }

        private void WaitForLaserSpawner2(float deltaTime)
        {
            if(CurrentSpawner.ReadyToDeactivate)
            {
                CurrentSpawner.DeactivateSelf();
                CurrentSpawner = null;
                StowUtil.UnstowX(CurrentLaser, StowUtil.LaserEnemyBulletStowX);
                CurrentLaser.SequenceActive = true;

                FrameBehavior = FrameBehaviors.WaitForLaser3;
            }
        }

        private void WaitForLaser3(float deltaTime)
        {
            if (!CurrentLaser.isActiveAndEnabled)
            {
                CurrentLaser.DeactivateSelf();
                FrameBehavior = FrameBehaviors.RunFireFrame1;
                FireSequence.ResetSelf();
            }
        }

        private void FireLaser()
        {
            var bullets = LaserEnemyFireStrategy.GetBullets(this);

            CurrentLaser = (LaserEnemyBullet) bullets[LaserEnemyFireStrategy.LaserIndex];
            CurrentSpawner = (LaserEnemyBulletSpawner) bullets[LaserEnemyFireStrategy.LaserSpawnerIndex];
        }
        private void ResetRotate()
        {
            SpaceUtil.GetWorldBoundsX(SpriteMap.Width, out float minX, out float maxX);
            float x = RandomUtil.Float(minX, maxX);
            float y = SpaceUtil.WorldMap.Bottom.y;

            Vector3 targetPosition = new Vector3(x, y);

            Vector3 positionDelta = targetPosition - transform.position;

            float startAngle = RotationDegrees;
            float endAngle = Vector2.SignedAngle(Vector3.right, positionDelta);

            if (startAngle == 0)
                startAngle = 360f;

            // endAngle will always be negative.
            endAngle += 360f;

            Rotate.SetAngleRange(startAngle, endAngle);
        }

        protected override void OnEnemyDeactivate()
        {
            CurrentSpawner?.DeactivateSelf();
        }
    }
}