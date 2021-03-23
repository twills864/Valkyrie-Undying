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

        [SerializeField]
        private SpriteRenderer Sprite = null;
        protected override ColorHandler DefaultColorHandler()
            => new SpriteColorHandler(Sprite);

        [SerializeField]
        private float FireTimerIntervalBase = GameConstants.PrefabNumber;
        [SerializeField]
        private float FadeInTime = GameConstants.PrefabNumber;
        [SerializeField]
        private float FadeInTimeFireDelay = GameConstants.PrefabNumber;

        private Vector2 Size { get; set; }
        public SpriteBoxMap BoxMap { get; private set; }

        private float FireSpeedModifier { get; set; }

        /// <summary>
        /// Othello's fire timer activates just slightly faster than real-time
        /// to account for differences in precision.
        /// </summary>
        private float FireSpeedPrecisionScale { get; set; } = 1.01f;
        private LoopingFrameTimer FireTimer { get; set; }

        private int Damage { get; set; }
        private float WorldCenterX { get; set; }

        public Vector3 FirePosition => BoxMap.Top;

        private FadeTo FadeTo { get; set; }

        /// <summary>
        /// Currently unused
        /// </summary>
        private int Level { get; set; }

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

        protected override void OnFrameRun(float deltaTime, float realDeltaTime)
        {
            FadeTo.RunFrame(deltaTime);

            transform.position = CalculateNewPosition(Player.Instance.transform.position);

            // Only attempt to increment the fire timer if it's not already activated.
            // This preserves the activation status for when the player fires their main cannon.
            if(!FireTimer.Activated)
                FireTimer.Increment(deltaTime * FireSpeedModifier);
        }

        public void Activate()
        {
            gameObject.SetActive(true);

            var targetAlpha = Alpha;
            Alpha = 0;
            FadeTo = new FadeTo(this, targetAlpha, FadeInTime);
        }

        public void LevelUp(int level, int damage, float fireSpeedModifier)
        {
            if (level == 1)
                Activate();

            Level = level;
            Damage = damage;
            FireSpeedModifier = fireSpeedModifier;
            FireTimer.ActivationInterval = FireTimerIntervalBase * fireSpeedModifier;
        }

        public void Fire()
        {
            if (gameObject.activeSelf && FireTimer.Activated)
            {
                var bullet = PoolManager.Instance.BulletPool.Get<OthelloBullet>(FirePosition, Level);
                bullet.DamageIncrease = Damage;
                FireTimer.TouchTimer();
            }
        }

        public void ResetFiretimer()
        {
            FireTimer.ActivateSelf();
        }

        protected sealed override void OnInit()
        {
            Instance = this;

            BoxMap = new SpriteBoxMap(this);

            var sprite = GetComponent<SpriteRenderer>();
            Size = sprite.size;

            WorldCenterX = SpaceUtil.WorldMap.Center.x;

            FireTimer = new LoopingFrameTimer(FireTimerIntervalBase);
            FireTimer.Elapsed = -FadeInTime - FadeInTimeFireDelay;
            FireSpeedModifier = FireSpeedPrecisionScale;
        }
    }
}
