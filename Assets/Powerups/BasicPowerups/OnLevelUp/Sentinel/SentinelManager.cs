using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.FireStrategies.PlayerFireStrategies;
using Assets.GameTasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Manages the bullets spawned by the Sentinel powerup.
    /// </summary>
    /// <inheritdoc/>
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
            for (int i = 0; i < sentinels.Length; i++)
            {
                SentinelBullet sentinel = sentinels[i];
                sentinel.Index = i;
                sentinel.DeactivateSelf();
            }
        }

        protected override void OnFrameRun(float deltaTime, float realDeltaTime)
        {
            //if (Level == 0)
            //    return;

            Rotation.AddAngle(deltaTime * AngularVelocity);

            if (RespawnTimer.UpdateActivates(deltaTime))
                ActivateRandomSentinel();
        }

        private void LateUpdate()
        {
            transform.position = Player.Position;

            float angle = Rotation;
            for (int i = 0; i < NumSentinel; i++)
            {
                var sentinel = (SentinelBullet)SentinelPool[i];
                if (sentinel.isActiveAndEnabled)
                {
                    var offset = (Vector3)MathUtil.VectorAtRadianAngle(angle, Radius);

                    var destination = transform.position + offset;
                    var ratio = sentinel.RadiusRatio;
                    var newPos = MathUtil.ScaledPositionBetween(transform.position, destination, ratio);
                    sentinel.transform.position = newPos;

                    sentinel.RepresentedVelocityTrackerLateUpdate();
                }

                angle += AngleDelta;
            }
        }

        /// <summary>
        /// Calculates the represented velocity of a given <paramref name="sentinel"/>.
        /// (Does not take the entrance animation into consideration.)
        /// </summary>
        /// <param name="sentinel">The sentinel to calculate the represented velocity for.</param>
        /// <returns>The sentinel's represented velocity.</returns>
        [Obsolete("Replaced with RepresentedVelocityTracker in SentinelBullet")]
        public Vector2 CalculateRepresentedVelocity(SentinelBullet sentinel)
        {
            int index = sentinel.Index;
            float angleOffset = index * MathUtil.Pi2f / NumSentinel;

            // Apply 90 degree offset to calculate correct velocity
            const float HardOffset = MathUtil.PiHalf;

            float angle = Rotation + angleOffset + HardOffset;

            const float Magnitude = 11f;
            Vector2 velocity = MathUtil.VectorAtRadianAngle(angle, Magnitude);

            return velocity;
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
                        //float newDistanceRatio = sentinel.CurrentRadius / Radius;
                        //sentinel.RadiusRatio = newDistanceRatio;
                        FireSentinelForward(sentinel);
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


        public void ActivateAllSentinels()
        {
            foreach (var bullet in InactiveBullets)
            {
                bullet.ActivateSelf();
                bullet.OnSpawn();
            }
        }

        public void ActivateRandomSentinel()
        {
            //const string TimerName = "ActivateTime";
            //var timer = Stopwatch.StartNew();

            if (RandomUtil.TryGetRandomElement(InactiveBullets, out PlayerBullet bullet))
            {
                bullet.ActivateSelf();
                bullet.OnSpawn();
            }
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
            angleDegrees %= FullCircle;

            int index = (int)(angleDegrees * NumSentinel / FullCircle);
            index = Mathf.Clamp(index, 0, NumSentinel - 1);

            PlayerBullet sentinel = SentinelPool[index];

            if (SpaceUtil.SpriteIsInBounds(sentinel))
                FireSentinelForward(sentinel);
        }

        private void FireSentinelForward(PlayerBullet bullet)
        {
            SentinelBullet sentinel = (SentinelBullet)bullet;
            SentinelProjectileBullet projectile = SentinelProjectileBullet.GetProjectile(sentinel);

            projectile.Velocity = new Vector2(0, projectile.Speed);
            projectile.OnSpawn();

            // Reset Sentinel entrance animation to finish the illusion that
            // the Sentinel was fired and immediately respawned.
            sentinel.ActivateSelf();
            sentinel.OnSpawn();
        }

        public void SentinelTriggered(SentinelBullet sentinel, Enemy enemy)
        {
            int index = sentinel.Index;
            int sentinelDamage = sentinel.Damage;

            var triggered = GetNextTriggeredSentinel(index).GetEnumerator();

            // enemy.CurrentHealth is the health after hitting the first Sentinel.
            int theoreticalHealth = enemy.CurrentHealth; // - sentinel.Damage;

            Vector2 enemyPosition = enemy.transform.position;
            Vector2 enemyVelocity = enemy.Velocity;


            while (theoreticalHealth > 0 && triggered.MoveNext())
            {
                SentinelBullet next = triggered.Current;
                SentinelProjectileBullet projectile = SentinelProjectileBullet.GetProjectile(next);

                Vector2 projPos = projectile.transform.position;
                projectile.Velocity = MathUtil.VelocityVector(projPos, enemyPosition, projectile.Speed)
                    + enemyVelocity;

                projectile.OnSpawn();

                theoreticalHealth -= sentinelDamage;
                next.DeactivateSelf();
            }
        }

        /// <summary>
        /// Gets each next-closest Sentinel to the Sentinel at a given <paramref name="startingIndex"/>.
        /// The offsets of each next Sentinel follow the pattern (<paramref name="startingIndex"/> +1), -1, +2, -2, +3, ...
        /// The offsets are adjusted to stay within the bounds of the array.
        /// </summary>
        /// <param name="startingIndex">The index of the triggered Sentinel.</param>
        /// <returns>Each next-closest Sentinel.</returns>
        private IEnumerable<SentinelBullet> GetNextTriggeredSentinel(int startingIndex)
        {
            int nextIndex;
            SentinelBullet nextSentinel;

            bool ShouldYield()
            {
                nextSentinel = (SentinelBullet) SentinelPool[nextIndex];
                return nextSentinel.isActiveAndEnabled;
            }

            const int MaxLevel = (NumSentinel / 2);
            for(int level = 1; level < MaxLevel; level++)
            {
                nextIndex = startingIndex + level;
                if (nextIndex >= NumSentinel) nextIndex -= NumSentinel;
                if (ShouldYield()) yield return nextSentinel;

                nextIndex = startingIndex - level;
                if (nextIndex < 0) nextIndex += NumSentinel;
                if (ShouldYield()) yield return nextSentinel;
            }

            // Fire most distant Sentinel if necessary
            if(MathUtil.IsEven(NumSentinel))
            {
                const int Half = (NumSentinel / 2);
                if (startingIndex < Half)
                    nextIndex = startingIndex + Half;
                else
                    nextIndex = startingIndex - Half;

                if (ShouldYield()) yield return nextSentinel;
            }
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
