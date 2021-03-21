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
        [SerializeField]
        private float FlyTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float RotateTime = GameConstants.PrefabNumber;

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
        private Sequence FlyInSequence { get; set; }

        private RotateTo Rotate { get; set; }
        private Sequence FireSequence { get; set; }

        private FrameBehaviorArray FrameBehaviors;

        public float WidthHalf { get; private set; }

        private LaserEnemyBullet CurrentLaser { get; set; }

        protected override void OnEnemyInit()
        {
            var sprite = GetComponent<SpriteRenderer>();
            WidthHalf = sprite.size.x * 0.5f;

            Rotator = new PositionRotator(this);
            FrameBehaviors = new FrameBehaviorArray(RunFlyInFrame, RunFireFrame, WaitForLaser);

            #region FlyInSequence

            MoveVertical = new MoveBy(this, Vector3.zero, FlyTime);
            MoveHorizontal = new MoveBy(this, Vector3.zero, FlyTime);

            var easeVertical = new EaseIn(MoveVertical);
            var easeHorizontal = new EaseOut(MoveHorizontal);

            var concurrence = new ConcurrentGameTask(this, easeVertical, easeHorizontal);
            //var changeBehavior = new GameTaskFunc(this, () => FrameBehaviors.BehaviorIndex++);

            var easeIn2 = new EaseIn(concurrence);

            FlyInSequence = new Sequence(easeIn2/*, changeBehavior*/);

            #endregion FlyInSequence

            #region FireLoop

            var resetRotate = new GameTaskFunc(this, ResetRotate);
            Rotate = new RotateTo(this, 0.0f, RotateTime);
            var fireLaser = new GameTaskFunc(this, FireLaser);
            //var changeBehavior2 = new GameTaskFunc(this, () => FrameBehaviors.BehaviorIndex++);
            FireSequence = new Sequence(resetRotate, Rotate, fireLaser/*, changeBehavior2*/);

            #endregion FireLoop
        }

        protected override void OnEnemySpawn()
        {
            Vector3 destination = SpaceUtil.RandomEnemyDestinationTopHalf(this);
            Vector3 difference = destination - transform.position;

            MoveX = difference.x;
            MoveY = difference.y;

            FrameBehaviors.ResetSelf();
            FlyInSequence.ResetSelf();
            FireSequence.ResetSelf();
        }

        protected override void OnEnemyFrame(float deltaTime)
        {
            DebugUI.SetDebugLabel("behavior", FrameBehaviors.CurrentBehavior.Method.Name);
            FrameBehaviors.CurrentBehavior(deltaTime);
            if (FrameBehaviors.BehaviorIndex == 3)
                System.Diagnostics.Debugger.Break();
        }

        private void RunFlyInFrame(float deltaTime)
        {
            Rotator.RunFrame(deltaTime);
            if (FlyInSequence.FrameRunFinishes(deltaTime))
            {
                FrameBehaviors.BehaviorIndex++;
                RunFireFrame(FlyInSequence.OverflowDeltaTime);
            }
        }
        private void RunFireFrame(float deltaTime)
        {
            if (FireSequence.FrameRunFinishes(deltaTime))
            {
                FrameBehaviors.BehaviorIndex++;
                //WaitForLaser(FlyInSequence.OverflowDeltaTime);
            }
        }

        private void WaitForLaser(float deltaTime)
        {

            FrameBehaviors.BehaviorIndex--;
            FireSequence.ResetSelf();

            //if(!CurrentLaser.HitBoxActive)
            //{
            //    FrameBehaviors.BehaviorIndex--;
            //    FireSequence.ResetSelf();
            //}
        }

        private void FireLaser()
        {
            DebugUI.SetDebugLabel("FIREDANGLE", RotationDegrees);
            LaserEnemyFireStrategy.GetBullets(this);

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

            //DebugUI.SetDebugLabel("StartAngle", startAngle);
            //DebugUI.SetDebugLabel("EndAngle", endAngle);
            //DebugUI.SetDebugLabel("From", transform.position);
            //DebugUI.SetDebugLabel("To", targetPosition);

            //DebugUtil.RedX(transform.position, RotateTime);
            //DebugUtil.RedX(targetPosition, RotateTime);

            //Debug.DrawLine(transform.position, targetPosition, Color.red, RotateTime);
        }

        protected override void OnDeath()
        {
            PoolManager.Instance.EnemyPool.GetRandomEnemy();
        }
    }
}