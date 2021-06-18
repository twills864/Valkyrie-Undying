using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.FireStrategies.PlayerFireStrategies;
using Assets.GameTasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.ObjectPooling;
using Assets.Pickups;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// A mirror self that fires bullets alongside the player.
    /// </summary>
    public class Othello : ValkyrieSprite
    {
        public static Othello Instance { get; set; }

        public override TimeScaleType TimeScale => TimeScaleType.Player;

        #region Prefabs

        [SerializeField]
        private SpriteRenderer _Sprite = null;

        [SerializeField]
        private float _FireTimerIntervalBase = GameConstants.PrefabNumber;
        [SerializeField]
        private float _FadeInTime = GameConstants.PrefabNumber;
        [SerializeField]
        private float _FadeInTimeFireDelay = GameConstants.PrefabNumber;
        [SerializeField]
        private float _BulletSpread = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private SpriteRenderer Sprite => _Sprite;

        private float FireTimerIntervalBase => _FireTimerIntervalBase;
        private float FadeInTime => _FadeInTime;
        private float FadeInTimeFireDelay => _FadeInTimeFireDelay;
        private float BulletSpread => _BulletSpread;

        #endregion Prefab Properties


        protected override ColorHandler DefaultColorHandler()
            => new SpriteColorHandler(Sprite);

        private Vector2 Size { get; set; }
        public SpriteBoxMap BoxMap { get; private set; }

        /// <summary>
        /// Othello's fire timer activates just slightly faster than real-time
        /// to account for differences in precision.
        /// </summary>
        private float FireSpeedPrecisionScale { get; set; } = 1.01f;
        private FrameTimerWithBuffer FireTimer { get; set; }

        private int Damage { get; set; }
        private float WorldCenterX { get; set; }

        public Vector3 FirePosition => BoxMap.Top;

        private FadeTo FadeTo { get; set; }

        /// <summary>
        /// Currently unused
        /// </summary>
        private int Level { get; set; }

        protected sealed override void OnInit()
        {
            Instance = this;

            BoxMap = new SpriteBoxMap(this);

            var sprite = GetComponent<SpriteRenderer>();
            Size = sprite.size;

            WorldCenterX = SpaceUtil.WorldMap.Center.x;

            FireTimer = new FrameTimerWithBuffer(FireTimerIntervalBase);
        }

        protected override void OnFrameRun(float deltaTime, float realDeltaTime)
        {
            FadeTo.RunFrame(deltaTime);

            transform.position = CalculateNewPosition(Player.Instance.transform.position);

            FireTimer.Increment(deltaTime);
        }

        /// <summary>
        /// Calculates the position that is the same as the player's current position,
        /// but with the X coordinate flipped across the center of the screen.
        /// </summary>
        /// <param name="playerPosition">The current position of the player.</param>
        /// <returns>The new position for Othello.</returns>
        private Vector3 CalculateNewPosition(Vector3 playerPosition)
        {
            float xDiff = (playerPosition.x - WorldCenterX) * 2f;
            float newX = playerPosition.x - xDiff;

            Vector3 ret = new Vector3(newX, playerPosition.y, 0);
            return ret;
        }

        public void LevelUp(int level, int damage)
        {
            if (level == 1)
                Activate();

            Level = level;
            Damage = damage;
        }

        public void Activate()
        {
            gameObject.SetActive(true);

            FireTimer.Reset();
            FireTimer.TotalTime = -FadeInTime - FadeInTimeFireDelay;

            var targetAlpha = Alpha;
            Alpha = 0;
            FadeTo = new FadeTo(this, targetAlpha, FadeInTime);
        }

        public void Fire(float fireStrategyOverflowDt)
        {
            if (gameObject.activeSelf && FireTimer.Activated)
            {
                var bullets = PoolManager.Instance.BulletPool.GetMany<OthelloBullet>(Level, FirePosition, Level);

                float offsetX = -(BulletSpread * (Level - 1) * 0.5f);
                for (int i = 0; i < bullets.Length; i++)
                {
                    OthelloBullet bullet = bullets[i];
                    bullet.OthelloDamage = Damage;

                    bullet.PositionX += offsetX;

                    bullet.RunFrame(fireStrategyOverflowDt, fireStrategyOverflowDt);

                    if (!SpaceUtil.PointIsInBounds(bullet.transform.position))
                        bullet.RunTask(GameTaskFunc.DeactivateSelf(bullet));

                    offsetX += BulletSpread;
                }

                FireTimer.TouchTimer();
            }
        }

        public void ResetFiretimer()
        {
            FireTimer.ActivateSelf();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (CollisionUtil.IsPickup(collision))
            {
                Pickup pickup = collision.GetComponent<Pickup>();
                pickup.PickUp();
            }
        }

        public void Kill()
        {
            gameObject.SetActive(false);
            Instance = null;
        }
    }
}
