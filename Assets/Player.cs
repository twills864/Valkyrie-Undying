using Assets.Bullets.EnemyBullets;
using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets
{
    /// <inheritdoc/>
    public class Player : FrameRunner
    {
        public static Player Instance { get; private set; }

        public override string LogTagColor => "#60D3FF";

        [SerializeField]
        private GameObject BloodlustAuraObject;

        private Rigidbody2D Body { get; set; }
        private Renderer Renderer { get; set; }
        private LineRenderer LineRenderer { get; set; }
        private SpriteRenderer BloodlustAuraSprite { get; set; }

        public Vector2 Size => Renderer.bounds.size;
        private TrackedBoxMap BoxMap { get; set; }

        [SerializeField]
        private float MobileYOffset;
        private static float MobileY;

        private float MinX { get; set; }
        private float MaxX { get; set; }


        #region Fire Speed

        public float FireSpeedScale => BloodlustSpeedScale;
        public float BloodlustSpeedScale { get; set; } = 1f;
        private FrameTimer BloodlustTimer { get; set; }

        #endregion Fire Speed

        private void Start()
        {
            BoxMap = new TrackedBoxMap(this);
            BloodlustTimer = FrameTimer.Default();
        }

        void Update()
        {
            if (Input.GetMouseButton(0))
                SetMobilePosition(SpaceUtil.WorldPositionUnderMouse());
            else if (Input.GetMouseButton(1))
                SetPosition(SpaceUtil.WorldPositionUnderMouse());
        }

        public void Init()
        {
            Instance = this;

            Body = GetComponent<Rigidbody2D>();
            Renderer = GetComponent<Renderer>();
            BloodlustAuraSprite = BloodlustAuraObject.GetComponent<SpriteRenderer>();
            LineRenderer = GetComponent<LineRenderer>();

            //var targetY = Camera.main.ScreenToWorldPoint(new Vector3(0, MobileYOffset));
            var heightHalf = Renderer.bounds.size.y * 0.5f;
            MobileY = SpaceUtil.WorldMap.Bottom.y + heightHalf + MobileYOffset;
            Enemy.FireHeightFloor = MobileY;

            SpaceUtil.GetWorldBoundsX(Size.x, out float _MinX, out float _MaxX);
            MinX = _MinX;
            MaxX = _MaxX;

            SetMobilePosition(Vector2.zero);
        }

        public void SetMobilePosition(Vector2 pos)
        {
            pos.x = Mathf.Clamp(pos.x, MinX, MaxX);
            Vector2 newPos = new Vector2(pos.x, MobileY);
            SetPosition(newPos);
        }
        public void SetPosition(Vector2 pos)
        {
            Body.transform.localPosition = pos;
        }

        public Vector2 FirePosition()
        {
            var ret = BoxMap.Top;
            return ret;
        }

        public override void RunFrame(float deltaTime)
        {
            if (!BloodlustTimer.Activated && BloodlustTimer.UpdateActivates(deltaTime))
                ResetBloodlust();
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