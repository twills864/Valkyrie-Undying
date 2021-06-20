using System;
using System.Linq;
using Assets.Bullets.EnemyBullets;
using Assets.Constants;
using Assets.FireStrategies.EnemyFireStrategies;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.UnityPrefabStructs;
using Assets.Util;
using UnityEngine;

namespace Assets.Enemies
{
    /// <summary>
    /// An enemy that flies to a random Y position, and then flies back and forth
    /// across the screen horizontally.
    /// </summary>
    /// <inheritdoc/>
    public class NomadEnemy : Enemy
    {
        public override AudioClip FireSound => SoundBank.GunPistol;

        #region Prefabs

        [SerializeField]
        private float _FlyInTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _TraverseScreenTime = GameConstants.PrefabNumber;

        [SerializeField]
        private VariantFireSpeed _VariantFireSpeed = default;

        #endregion Prefabs


        #region Prefab Properties

        private float FlyInTime => _FlyInTime;
        private float TraverseScreenTime => _TraverseScreenTime;

        protected VariantFireSpeed VariantFireSpeed => _VariantFireSpeed;
        protected float FireSpeed => _VariantFireSpeed.FireSpeed;
        protected float FireSpeedVariance => _VariantFireSpeed.FireSpeedVariance;

        #endregion Prefab Properties


        protected override EnemyFireStrategy InitialFireStrategy()
            => new NomadEnemyStrategy(VariantFireSpeed);

        public LoopingFrameTimer FireTimer => FireStrategy.FireTimer;

        private Vector2 InitialSize { get; set; }

        private MoveTo InitialFlyIn { get; set; }
        private EaseIn InitialFlyInEase { get; set; }

        private MoveTo MoveRight { get; set; }
        private MoveTo MoveLeft { get; set; }

        private Sequence RepeatSequence { get; set; }
        private RepeatForever RepeatMoves { get; set; }

        protected override void OnEnemyInit()
        {
            InitialSize = SpriteMap.Size;

            InitialFlyIn = MoveTo.Default(this, FlyInTime);
            InitialFlyInEase = new EaseIn(InitialFlyIn);

            float xRight = SpaceUtil.WorldMap.Right.x - (InitialSize.x * 0.5f);
            Vector3 destinationRight = new Vector3(xRight, 0);

            float xLeft = SpaceUtil.WorldMap.Left.x + (InitialSize.x * 0.5f);
            Vector3 destinationLeft = new Vector3(xLeft, 0);

            MoveRight = new MoveTo(this, destinationLeft, destinationRight, TraverseScreenTime);
            MoveLeft = new MoveTo(this, destinationRight, destinationLeft, TraverseScreenTime);

            var easeRight = new EaseInOut(MoveRight);
            var easeLeft = new EaseInOut(MoveLeft);

            RepeatSequence = new Sequence(easeRight, easeLeft);
            RepeatMoves = new RepeatForever(RepeatSequence);
        }

        protected override void OnEnemyActivate()
        {

        }

        protected override void OnEnemySpawn()
        {
            float maxY = SpaceUtil.WorldMap.Top.y - InitialSize.y;
            float targetY = RandomUtil.Float(SpaceUtil.WorldMap.Center.y, maxY);

            bool flyingLeft = PositionX < SpaceUtil.WorldMap.Center.x;
            float firstDestinationX = flyingLeft ? MoveLeft.Destination.x : MoveRight.Destination.x;
            Vector3 firstDestination = new Vector3(firstDestinationX, targetY);

            InitialFlyIn.ReinitializeMove(transform.position, firstDestination);

            MoveRight.StartPosition = MoveRight.StartPosition.WithY(targetY);
            MoveRight.Destination = MoveRight.Destination.WithY(targetY);

            MoveLeft.StartPosition = MoveLeft.StartPosition.WithY(targetY);
            MoveLeft.Destination = MoveLeft.Destination.WithY(targetY);

            InitialFlyInEase.ResetSelf();
            RepeatMoves.ResetSelf();

            if (!flyingLeft)
                RepeatSequence.SkipCurrentTask();
        }

        protected override void OnEnemyFrame(float deltaTime, float realDeltaTime)
        {
            if (!InitialFlyInEase.IsFinished)
                InitialFlyInEase.RunFrame(deltaTime);
            else
                RepeatMoves.RunFrame(deltaTime);

            if (FireTimer.UpdateActivates(deltaTime))
                FireBullets();
        }

        private void FireBullets()
        {
            var bullets = FireStrategy.GetBullets(FirePosition);
            PlayFireSound();
        }
    }
}
