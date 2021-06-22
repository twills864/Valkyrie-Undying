using System;
using System.Linq;
using Assets.Bullets.EnemyBullets;
using Assets.Components;
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
        private float _InitialTravelSpeed = GameConstants.PrefabNumber;

        [SerializeField]
        private float _RotateTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float _TraverseScreenTime = GameConstants.PrefabNumber;

        [SerializeField]
        private VariantFireSpeed _VariantFireSpeed = default;

        [SerializeField]
        private float _HoverDistance = GameConstants.PrefabNumber;

        [SerializeField]
        private float _HoverDistanceTime = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private float InitialTravelSpeed => _InitialTravelSpeed;
        private float TraverseScreenTime => _TraverseScreenTime;

        private float RotateTime => _RotateTime;

        protected VariantFireSpeed VariantFireSpeed => _VariantFireSpeed;
        protected float FireSpeed => _VariantFireSpeed.FireSpeed;
        protected float FireSpeedVariance => _VariantFireSpeed.FireSpeedVariance;

        private float HoverDistance => _HoverDistance;
        private float HoverDistanceTime => _HoverDistanceTime;

        #endregion Prefab Properties


        protected override EnemyFireStrategy InitialFireStrategy()
            => new NomadEnemyStrategy(VariantFireSpeed);

        public LoopingFrameTimer FireTimer => FireStrategy.FireTimer;

        private Vector2 InitialSize { get; set; }

        private MoveTo InitialFlyIn { get; set; }
        private EaseIn InitialFlyInEase { get; set; }

        private Vector3 InitialDestination
        {
            get => InitialFlyIn.Destination;
            set
            {
                Vector3 newDistance = value - transform.position;

                // time = distance / speed
                const float FlatAddition = 0.25f;
                InitialFlyInEase.Duration = (newDistance.magnitude / InitialTravelSpeed) + FlatAddition;

                InitialFlyInEase.ResetSelf();
                InitialFlyIn.ReinitializeMove(transform.position, value);
            }
        }

        private RotateTo RotateDown { get; set; }
        private EaseInOut RotateDownEase { get; set; }

        private MoveTo MoveRight { get; set; }
        private MoveTo MoveLeft { get; set; }

        private Sequence RepeatSequence { get; set; }
        private RepeatForever RepeatMoves { get; set; }

        private RepeatForever HoverRepeat { get; set; }

        protected override void OnEnemyInit()
        {
            // Swap X and Y, since sprite spawns at 90 degree angle.
            InitialSize = SpriteMap.Size.WithX(SpriteMap.Size.y).WithY(SpriteMap.Size.x);

            InitialFlyIn = MoveTo.Default(this, 1.0f);
            InitialFlyInEase = new EaseIn(InitialFlyIn);

            const float AngleDown = 270f;
            RotateDown = new RotateTo(this, 0f, AngleDown, RotateTime);
            RotateDownEase = new EaseInOut(RotateDown);

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


            Vector3 hoverDistance = new Vector3(0, HoverDistance);
            var hoverDown = new MoveBy(this, -hoverDistance, HoverDistanceTime);
            var hoverUp = new MoveBy(this, hoverDistance, HoverDistanceTime);

            var hoverDownEase = new EaseInOut(hoverDown);
            var hoverUpEase = new EaseInOut(hoverUp);

            HoverRepeat = new RepeatForever(hoverDownEase, hoverUpEase);
        }

        protected override void OnEnemySpawn()
        {
            float maxY = SpaceUtil.WorldMap.Top.y - InitialSize.y;
            float targetY = RandomUtil.Float(SpaceUtil.WorldMap.Center.y, maxY);

            bool flyingLeft = PositionX < SpaceUtil.WorldMap.Center.x;
            float firstDestinationX = flyingLeft ? MoveLeft.Destination.x : MoveRight.Destination.x;
            Vector3 firstDestination = new Vector3(firstDestinationX, targetY);

            RotationDegrees = MathUtil.AngleDegreesFromPoints(transform.position, firstDestination);

            InitialDestination = firstDestination;

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
            {
                bool flyInFinished = InitialFlyInEase.FrameRunFinishes(deltaTime);

                if (flyInFinished)
                {
                    RotateDown.StartRotationDegrees = RotationDegrees;
                    RotateDownEase.ResetSelf();
                    HoverRepeat.ResetSelf();
                }
            }
            else
            {
                HoverRepeat.RunFrame(deltaTime);

                if (!RotateDownEase.IsFinished)
                    RotateDownEase.RunFrame(deltaTime);
                else
                {
                    RepeatMoves.RunFrame(deltaTime);

                    if (FireTimer.UpdateActivates(deltaTime))
                        FireBullets();
                }
            }
        }

        private void FireBullets()
        {
            var bullets = FireStrategy.GetBullets(FirePosition);
            PlayFireSound();
        }
    }
}
