using Assets.Bullets.EnemyBullets;
using Assets.Constants;
using Assets.Enemies;
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

        public override string LogTagColor => "#60D3FF";
        public override GameTaskType TaskType => GameTaskType.Player;

        [SerializeField]
        private SpriteRenderer Sprite = null;
        protected override ColorHandler DefaultColorHandler()
            => new SpriteColorHandler(Sprite);

        [SerializeField]
        private GameObject BloodlustAuraObject = null;

        private Rigidbody2D Body { get; set; }
        private LineRenderer LineRenderer { get; set; }
        private SpriteRenderer BloodlustAuraSprite { get; set; }

        public Vector2 Size => Sprite.bounds.size;
        public SpriteBoxMap SpriteMap { get; private set; }
        public ColliderBoxMap ColliderMap { get; private set; }

        [SerializeField]
        private float MobileYOffset = GameConstants.PrefabNumber;
        private static float MobileY;

        private float MinX { get; set; }
        private float MaxX { get; set; }

        private Vector3 LastCursorPosition { get; set; }


        #region Fire Speed

        public float FireSpeedScale => BloodlustSpeedScale;
        public float BloodlustSpeedScale { get; set; } = 1f;
        private FrameTimer BloodlustTimer { get; set; }

        #endregion Fire Speed

        [SerializeField]
        private float _victimMarkerDistance = GameConstants.PrefabNumber;
        public float VictimMarkerDistance => _victimMarkerDistance;
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


        private void Start()
        {
            SpriteMap = new SpriteBoxMap(this);
            ColliderMap = new ColliderBoxMap(this);
            BloodlustTimer = FrameTimer.Default();
        }

        protected sealed override void OnInit()
        {
            Instance = this;

            Body = GetComponent<Rigidbody2D>();
            BloodlustAuraSprite = BloodlustAuraObject.GetComponent<SpriteRenderer>();
            LineRenderer = GetComponent<LineRenderer>();

            //var targetY = Camera.main.ScreenToWorldPoint(new Vector3(0, MobileYOffset));
            var heightHalf = Size.y * 0.5f;
            MobileY = SpaceUtil.WorldMap.Bottom.y + heightHalf + MobileYOffset;
            Enemy.FireHeightFloor = MobileY;

            SpaceUtil.GetWorldBoundsX(Size.x, out float _MinX, out float _MaxX);
            MinX = _MinX;
            MaxX = _MaxX;

            SetMobilePosition(Vector3.zero);
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
            Body.transform.localPosition = pos;
        }

        public Vector3 FirePosition()
        {
            var ret = SpriteMap.Top;
            return ret;
        }

        protected override void OnFrameRun(float deltaTime)
        {
            if (!BloodlustTimer.Activated && BloodlustTimer.UpdateActivates(deltaTime))
                ResetBloodlust();

            HandleMovement();
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

        /// <summary>
        /// Handles player getting hit by a bullet.
        /// Returns true if the bullet should behave as if it successfully hits the player.
        /// </summary>
        /// <returns>True if the bullet should behave as if it successfully hits the player;
        /// false if the player avoids the collision, e.g. with a powerup.</returns>
        public bool CollideWithBullet(EnemyBullet bullet)
        {
            GameManager.Instance._PowerupManager.OnGetHit();
            GameManager.Instance.CreateFleetingText("Ow", transform.position);
            return true;
        }

        /// <summary>
        /// Handles player colliding with an enemy.
        /// Returns true if the enemy should behave as if it successfully collides with the player.
        /// </summary>
        /// <returns>True if the enemy should behave as if it successfully collides with the player;
        /// false if the player avoids the collision, e.g. with a powerup.</returns>
        public bool CollideWithEnemy(Enemy enemy)
        {
            if (enemy.name != DebugUtil.DebugEnemyName)
            {
                GameManager.Instance._PowerupManager.OnGetHit();
                GameManager.Instance.CreateFleetingText("Ow!", transform.position);
                return true;
            }
            else
            {
                GameManager.Instance.CreateFleetingText("No.", transform.position);
                return false;
            }
        }
    }
}