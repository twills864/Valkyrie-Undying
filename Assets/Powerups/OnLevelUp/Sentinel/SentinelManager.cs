using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class SentinelManager : ValkyrieSprite
    {
        #region Prefabs

        [SerializeField]
        private float _AngularVelocity = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private float AngularVelocity => _AngularVelocity;

        #endregion Prefab Properties


        public static SentinelManager Instance { get; set; }

        const int NumSentinel = 8;
        const float AngleDelta = MathUtil.Pi2f / NumSentinel;

        public override TimeScaleType TimeScale => TimeScaleType.Player;
        protected override ColorHandler DefaultColorHandler()
            => new NullColorHandler();

        private LoopingFrameTimer RespawnTimer { get; } = LoopingFrameTimer.Default();

        private int Level { get; set; }
        private float Radius { get; set; }
        private CounterClockwiseRotation Rotation;

        private ObjectPool<PlayerBullet> SentinelPool { get; set; }
        private IEnumerable<SentinelBullet> Sentinels
            => SentinelPool.Select(x => (SentinelBullet)x);
        private IEnumerable<PlayerBullet> InactiveBullets
            => SentinelPool.Where(x => !x.isActiveAndEnabled);

        private ObjectPool<PlayerBullet> SentinelProjectilePool { get; set; }

        protected sealed override void OnInit()
        {
            Instance = this;

            SentinelPool = PoolManager.Instance.BulletPool.GetPool<SentinelBullet>();
            SentinelProjectilePool = PoolManager.Instance.BulletPool.GetPool<SentinelProjectileBullet>();

            var sentinels = SentinelPool.GetMany<SentinelBullet>(NumSentinel);
            foreach (var sentinel in sentinels)
                sentinel.DeactivateSelf();
        }

        public void ActivateRandomSentinel()
        {
            //const string TimerName = "ActivateTime";
            //var timer = Stopwatch.StartNew();

            if (RandomUtil.TryGetRandomElement(InactiveBullets, out PlayerBullet bullet))
                bullet.ActivateSelf();
            else if (Level == 1)
                FireRandomSentinel();
            else
                FireBestSentinel();

            #region Deprecated array implementation
            //var inactiveSentinels = InactiveSentinels.ToArray();

            //if (RandomUtil.TryGetRandomElement(inactiveSentinels, out PlayerBullet bullet))
            //    bullet.ActivateSelf();
            #endregion Deprecated array implementation

            //timer.Stop();
            //DebugUI.SetDebugLabel(TimerName, timer.Elapsed);
        }

        private void FireRandomSentinel()
        {
            var sentinels = SentinelPool.Where(x => SpaceUtil.SpriteIsInBounds(x));
            if (RandomUtil.TryGetRandomElement(sentinels, out PlayerBullet bullet))
                FireSentinelForward(bullet);
        }

        /// <summary>
        /// Fires the Sentinel currently directly in front of the player.
        /// </summary>
        private void FireBestSentinel()
        {
            const float FullCircle = 360f;
            const float RightAngle = 90f;
            const float HalfSentinelAngle = FullCircle / NumSentinel * 0.5f;

            float angleDegrees = (FullCircle + RightAngle + HalfSentinelAngle);
            angleDegrees -= (Rotation.Angle * Mathf.Rad2Deg);
            angleDegrees %= 360f;

            int index = (int)(angleDegrees * NumSentinel / 360f);
            index = Mathf.Clamp(index, 0, NumSentinel - 1);

            PlayerBullet sentinel = SentinelPool[index];

            if (SpaceUtil.SpriteIsInBounds(sentinel))
                FireSentinelForward(sentinel);
        }

        private void FireSentinelForward(PlayerBullet bullet)
        {
            SentinelBullet sentinel = (SentinelBullet)bullet;
            SentinelProjectileBullet projectile = (SentinelProjectileBullet)SentinelProjectilePool.Get();

            projectile.transform.position = sentinel.transform.position;
            projectile.Velocity = new Vector2(0, projectile.Speed);
            projectile.SentinelProjectileDamage = sentinel.Damage;

            // Reset Sentinel entrance animation to finish the illusion that
            // the Sentinel was fired and immediately respawned.
            sentinel.ActivateSelf();
        }

        public void ActivateAllSentinels()
        {
            foreach (var bullet in InactiveBullets)
                bullet.ActivateSelf();
        }

        protected override void OnFrameRun(float deltaTime, float realDeltaTime)
        {
            //if (Level == 0)
            //    return;

            Rotation.AddAngle(deltaTime * AngularVelocity);

            if (RespawnTimer.UpdateActivates(deltaTime))
                ActivateRandomSentinel();

            float angle = Rotation;
            for(int i = 0; i < NumSentinel; i++)
            {
                var sentinel = (SentinelBullet) SentinelPool[i];
                if(sentinel.isActiveAndEnabled)
                {
                    var offset = (Vector3) MathUtil.VectorAtRadianAngle(angle, Radius);

                    var destination = transform.position + offset;
                    var ratio = sentinel.RadiusRatio;
                    var newPos = MathUtil.ScaledPositionBetween(transform.position, destination, ratio);
                    sentinel.transform.position = newPos;
                }

                angle += AngleDelta;
            }
        }

        private void LateUpdate()
        {
            transform.position = Player.Instance.transform.position;
        }

        public void LevelUp(int level, float radius, float respawnInterval)
        {
            Instance.gameObject.SetActive(true);

            Level = level;

            RespawnTimer.ActivationInterval = respawnInterval;
            RespawnTimer.Reset();

            Radius = radius;

            if (level > 1)
            {
                foreach (var sentinel in Sentinels)
                {
                    if (sentinel.isActiveAndEnabled)
                    {
                        float newDistanceRatio = sentinel.CurrentRadius / Radius;
                        sentinel.RadiusRatio = newDistanceRatio;
                    }

                    sentinel.MaxRadius = Radius;
                }
            }
            else
            {
                foreach (var sentinel in Sentinels)
                    sentinel.MaxRadius = Radius;
            }

            ActivateAllSentinels();
        }

        public void Kill()
        {
            foreach (var sentinel in Sentinels)
            {
                if (sentinel.isActiveAndEnabled)
                    sentinel.DeactivateSelf();
            }

            gameObject.SetActive(false);
        }
    }
}
