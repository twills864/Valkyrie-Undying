using System;
using Assets.Bullets.EnemyBullets;
using Assets.Bullets.PlayerBullets;
using Assets.ColorManagers;
using Assets.Constants;
using Assets.Enemies;
using Assets.FireStrategyManagers;
using Assets.GameTasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.ObjectPooling;
using Assets.Powerups;
using Assets.UI;
using Assets.Util;
using UnityEngine;

namespace Assets
{
    /// <inheritdoc/>
    public class Player : ValkyrieSprite, IVictimHost
    {
        public static Player Instance { get; private set; }
        private static float MobileY { get; set; }

        public override string LogTagColor => "#60D3FF";
        public override TimeScaleType TimeScale => TimeScaleType.Player;

        protected override ColorHandler DefaultColorHandler()
            => new SpriteColorHandler(Sprite);

        #region Prefabs

        [SerializeField]
        private SpriteRenderer _Sprite = null;

        [SerializeField]
        private GameObject _BloodlustAuraObject = null;

        [SerializeField]
        private MortarGuide _MortarGuide = null;

        [SerializeField]
        private float _MobileYOffset = GameConstants.PrefabNumber;

        [SerializeField]
        private float _VictimMarkerDistance = GameConstants.PrefabNumber;

        [SerializeField]
        private LineRenderer _LaserPointer = null;

        [SerializeField]
        private PlayerIFrames _PlayerIFrames;

        #endregion Prefabs


        #region Prefab Properties

        public SpriteRenderer Sprite => _Sprite;
        private GameObject BloodlustAuraObject => _BloodlustAuraObject;
        private MortarGuide MortarGuide => _MortarGuide;

#if UNITY_EDITOR
        private float MobileYOffset => _MobileYOffset;
#else
        private float MobileYOffset => _MobileYOffset * 2.5f;
#endif
        public float VictimMarkerDistance => _VictimMarkerDistance;
        private LineRenderer LaserPointer => _LaserPointer;
        private PlayerIFrames PlayerIFrames => _PlayerIFrames;

        #endregion Prefab Properties


        private SpriteRenderer BloodlustAuraSprite { get; set; }

        public Vector2 Size => Sprite.bounds.size;
        public SpriteBoxMap SpriteMap { get; private set; }
        public ColliderBoxMap ColliderMap { get; private set; }

        public Collider2D Collider { get; private set; }

        private float MinX { get; set; }
        private float MaxX { get; set; }

        private Vector3 LastCursorPosition { get; set; }

        public Vector3 FirePosition => SpriteMap.Top;

#region Fire Speed

        public float FireSpeedScale => BloodlustSpeedScale;
        public float BloodlustSpeedScale { get; set; } = 1f;
        private FrameTimer BloodlustTimer { get; set; }

#endregion Fire Speed

#region Victim

        private VictimMarker _victimMarker;
        public VictimMarker VictimMarker
        {
            get => _victimMarker;
            set
            {
                _victimMarker = value;
                if (_victimMarker != null)
                    _victimMarker.Host = this;
            }
        }

#endregion Victim

#region Mortar

        public FrameTimerWithBuffer MortarFireTimer { get; private set; }
        public bool ShouldDrawMortar { get; set; }

#endregion Mortar

        private void Start()
        {
            SpriteMap = new SpriteBoxMap(this);
            ColliderMap = new ColliderBoxMap(this);
            Collider = ColliderMap.Collider;
            BloodlustTimer = FrameTimer.Default();
        }

        public void Init(in PlayerFireStrategyManager fireStrategyManager)
        {
            Init();

            float mortarTime = fireStrategyManager.BaseFireSpeed * 0.4f;
            MortarFireTimer = new FrameTimerWithBuffer(mortarTime, mortarTime + FrameTimerWithBuffer.DefaultBuffer);
        }

        protected sealed override void OnInit()
        {
            Instance = this;

            IsAlive = true;

            BloodlustAuraSprite = BloodlustAuraObject.GetComponent<SpriteRenderer>();

            LaserPointer.SetPosition(1, new Vector3(0, SpaceUtil.WorldMap.Height));

            //var targetY = Camera.main.ScreenToWorldPoint(new Vector3(0, MobileYOffset));
            var heightHalf = Size.y * 0.5f;

            MobileY = SpaceUtil.WorldMap.Bottom.y + heightHalf + MobileYOffset;
            Enemy.FireHeightFloor = MobileY;

            SpaceUtil.GetWorldBoundsX(Size.x, out float _MinX, out float _MaxX);
            MinX = _MinX;
            MaxX = _MaxX;

            SetMobilePosition(Vector3.zero);

            InIFrames = false;
            InitIFramesSequence();

            //var startDelay = new Delay(this, 0.5f);
            //var scaleTo = new ScaleTo(this, 3.0f, 1.5f);
            //var fadeOut = new FadeTo(this, 1.0f, 0f, 1.5f);
            //var fadeIn = new FadeTo(this, 0.0f, 0.6f, 1f);

            //var rotateFrom = new RotateTo(this, 0f, 90f, 1.0f);
            //var rotateTo = new RotateTo(this, 90f, 0f, 1.0f);
            //var delay = new Delay(this, 1.0f);
            //var seq = new Sequence(rotateFrom, rotateTo, delay);
            //var repeat = new RepeatForever(seq);

            //var endless = new EndlessSequence(repeat, startDelay, scaleTo, fadeOut, fadeIn);
            //this.RunTask(endless);




            //PositionX = SpaceUtil.WorldMap.Left.x / 2;

            //var start = new Vector3(SpaceUtil.WorldMap.Left.x / 2, PositionY);
            //var end = new Vector3(SpaceUtil.WorldMap.Right.x / 2, PositionY);
            //var move1 = new MoveTo(this, start, end, 2.0f);
            //var move2 = new MoveTo(this, end, start, 2.0f);

            //var ease1 = new EaseInOut(move1);
            //var ease2 = new EaseInOut(move2);
            //var sequence = new Sequence(ease1, ease2);
            //var repeate = new RepeatForever(sequence);
            //var endless = new EndlessSequence(repeate);

            //RunTask(endless);
        }

        public void SetMobilePosition(float posX)
        {
            posX = Mathf.Clamp(posX, MinX, MaxX);
            Vector3 newPos = new Vector3(posX, MobileY);
            SetPosition(newPos);
        }
        public void SetMobilePosition(Vector3 pos)
        {
            SetMobilePosition(pos.x);
        }
        public void SetPosition(Vector3 pos)
        {
            transform.localPosition = pos;
            SentinelManager.Instance.transform.position = pos;
        }

        protected override void OnFrameRun(float deltaTime, float realDeltaTime)
        {
            if (!BloodlustTimer.Activated && BloodlustTimer.UpdateActivates(deltaTime))
                ResetBloodlust();

            MortarFireTimer.Increment(deltaTime);

            HandleMovement();

            if (ShouldDrawMortar)
                MortarGuide.DrawMortar();

            IFramesSequence.RunFrame(realDeltaTime * RetributionTimeScale);
        }

        private void HandleMovement()
        {
            if (Input.GetMouseButtonDown(0))
                LastCursorPosition = SpaceUtil.WorldPositionUnderMouse();
            else if (Input.GetMouseButton(0))
            {
                Vector3 thisCursorPosition = SpaceUtil.WorldPositionUnderMouse();

                if (thisCursorPosition != LastCursorPosition)
                {
                    Vector3 delta = thisCursorPosition - LastCursorPosition;

                    var newX = transform.position.x + delta.x;
                    SetMobilePosition(newX);

                    LastCursorPosition = thisCursorPosition;
                }
            }
            else if (Input.GetMouseButton(1))
                SetPosition(SpaceUtil.WorldPositionUnderMouse());
        }

#region Damage

        /// <summary>
        /// Whether or not the player is currently invincible due to being recently damaged.
        /// </summary>
        public bool InIFrames { get; set; }

        public bool IsAlive { get; set; }

        private Sequence IFramesSequence { get; set; }

        private void InitIFramesSequence()
        {
            const float BaseAlpha = 1.0f;
            float blinkOutAlpha = PlayerIFrames.BlinkFadeOutAlpha;
            float blinkInAlpha = PlayerIFrames.BlinkFadeInAlpha;

            var blinkOut = new GameTaskFunc(this, () => Alpha = blinkOutAlpha);
            var blinkIn = new GameTaskFunc(this, () => Alpha = blinkInAlpha);

            var standardDelay = new Delay(this, PlayerIFrames.StandardBlinks.OneBlinkTime);
            var standardBlinkSequence = new Sequence(blinkOut, standardDelay, blinkIn, standardDelay);
            var standardBlinks = new Repeat(standardBlinkSequence, PlayerIFrames.StandardBlinks.NumBlinks);


            var finalDelay = new Delay(this, PlayerIFrames.FinalBlinks.OneBlinkTime);
            var finalBlinkSequence = new Sequence(blinkOut, finalDelay, blinkIn, finalDelay);
            var finalBlinks = new Repeat(finalBlinkSequence, PlayerIFrames.FinalBlinks.NumBlinks);

            var restoreAlpha = new GameTaskFunc(this, () => Alpha = BaseAlpha);

            var gracePeriod = new Delay(this, PlayerIFrames.GracePeriodTime);

            var restoreMortality = new GameTaskFunc(this, () => InIFrames = false);

            IFramesSequence = new Sequence(standardBlinks, finalBlinks, restoreAlpha, gracePeriod, restoreMortality);

            // Prevent sequence from running at start
            IFramesSequence.FinishSelf();
        }

        /// <summary>
        /// Returns true if the given <paramref name="collider"/> collides with the player.
        /// </summary>
        /// <param name="collider">The Collider2D to test.</param>
        /// <returns>True if the player collides with the given collider; False otherwise.</returns>
        public bool CollidesWithCollider(Collider2D collider)
        {
            bool ret = Collider.IsTouching(collider);
            return ret;
        }

        /// <summary>
        /// Handles player getting hit by a bullet.
        /// Returns true if the bullet should behave as if it successfully hits the player.
        /// </summary>
        /// <returns>True if the bullet should behave as if it successfully hits the player;
        /// false if the player avoids the collision, e.g. with a powerup.</returns>
        public bool CollidesWithBullet(EnemyBullet bullet)
        {
            OnHit();
            return true;
        }

        /// <summary>
        /// Handles player colliding with an enemy.
        /// Returns true if the enemy should behave as if it successfully collides with the player.
        /// </summary>
        /// <returns>True if the enemy should behave as if it successfully collides with the player;
        /// false if the player avoids the collision, e.g. with a powerup.</returns>
        public bool CollidesWithEnemy(Enemy enemy)
        {
            if (enemy.name != DebugUtil.DebugEnemyName)
            {
                OnHit();
                return true;
            }
            else
            {
                GameManager.Instance.CreateFleetingText("No.", transform.position);
                return false;
            }
        }

        private void OnHit()
        {
            if (!InIFrames)
                TakeDamage();
        }

        private void TakeDamage()
        {
            GameManager.Instance.OnGetHit();
            RetributionBullet.StartRetribution(ColliderMap.Center);

            InIFrames = true;
            IFramesSequence.ResetSelf();
        }

        public void Kill()
        {
            IsAlive = false;

            gameObject.SetActive(false);
            BloodlustAuraObject.SetActive(false);
            MortarGuide.gameObject.SetActive(false);
            VictimMarker?.StartDeactivation();
            BfgBulletSpawner.Instance.DeactivateSelf();
        }

#endregion Damage

#region Bloodlust

        private void ResetBloodlust()
        {
            BloodlustSpeedScale = 1.0f;
            BloodlustAuraObject.gameObject.SetActive(false);
        }

        public void SetBloodlust(float duration, float speedScale)
        {
            BloodlustTimer = new FrameTimer(duration);
            BloodlustSpeedScale = speedScale;

            BloodlustAuraObject.gameObject.SetActive(true);
        }

#endregion Bloodlust
    }
}