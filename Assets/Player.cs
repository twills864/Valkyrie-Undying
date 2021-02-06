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

        Rigidbody2D Body => GetComponent<Rigidbody2D>();
        Renderer Renderer => GetComponent<Renderer>();
        LineRenderer LineRenderer => GetComponent<LineRenderer>();

        public Vector2 Size => Renderer.bounds.size;
        private TrackedBoxMap BoxMap { get; set; }

        [SerializeField]
        private float MobileYOffset = 100;
        private static float MobileY;

        public override string LogTagColor => "#60D3FF";

        private float MinX { get; set; }
        private float MaxX { get; set; }

        private void Start()
        {
            BoxMap = new TrackedBoxMap(this);
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

            var targetY = Camera.main.ScreenToWorldPoint(new Vector3(0, MobileYOffset));
            MobileY = targetY.y;

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
            throw new System.NotImplementedException();
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