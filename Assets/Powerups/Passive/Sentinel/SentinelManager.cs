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
        public static SentinelManager Instance { get; set; }

        const int NumSentinel = 8;
        const float AngleDelta = MathUtil.Pi2f / NumSentinel;

        public override GameTaskType TaskType => GameTaskType.Player;
        protected override ColorHandler DefaultColorHandler()
            => new NullColorHandler();

        [SerializeField]
        private float AngularVelocity = GameConstants.PrefabNumber;

        private LoopingFrameTimer RespawnTimer { get; } = LoopingFrameTimer.Default();

        private int Level { get; set; }
        private float Radius { get; set; }
        private CounterClockwiseRotation Rotation;

        private ObjectPool<PlayerBullet> SentinelPool { get; set; }
        private IEnumerable<SentinelBullet> Sentinels
            => SentinelPool.Select(x => (SentinelBullet)x);
        private IEnumerable<PlayerBullet> InactiveBullets
            => SentinelPool.Where(x => !x.isActiveAndEnabled);

        protected sealed override void OnInit()
        {
            Instance = this;

            SentinelPool = PoolManager.Instance.BulletPool.GetPool<SentinelBullet>();

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

            #region Deprecated array implementation
            //var inactiveSentinels = InactiveSentinels.ToArray();

            //if (RandomUtil.TryGetRandomElement(inactiveSentinels, out PlayerBullet bullet))
            //    bullet.ActivateSelf();
            #endregion Deprecated array implementation

            //timer.Stop();
            //DebugUI.SetDebugLabel(TimerName, timer.Elapsed);
        }

        public void ActivateAllSentinels()
        {
            foreach (var bullet in InactiveBullets)
                bullet.ActivateSelf();
        }

        protected override void OnFrameRun(float deltaTime)
        {
            if (Level == 0)
                return;

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

        public void LevelUp(int level, float radius, float respawnInterval)
        {
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
    }
}
