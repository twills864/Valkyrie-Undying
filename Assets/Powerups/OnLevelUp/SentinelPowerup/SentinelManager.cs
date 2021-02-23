using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.FireStrategies.PlayerFireStrategies;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    public class SentinelManager : FrameRunner
    {
        public static SentinelManager Instance { get; set; }
        const int NumSentinel = 1;
        readonly float AngleDelta = MathUtil.Pi2 / NumSentinel;

        [SerializeField]
        private float AngularVelocity;

        private LoopingFrameTimer FireTimer = LoopingFrameTimer.Default();

        private int Level { get; set; }
        private float Radius { get; set; }
        private float Rotation { get; set; }

        private ObjectPool<PlayerBullet> SentinelPool;
        private IEnumerable<SentinelBullet> Sentinels
            => SentinelPool.Select(x => (SentinelBullet)x);

        public void Init()
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

            SentinelPool[0].ActivateSelf();
            return;

            var inactiveSentinels = SentinelPool.Where(x => !x.isActiveAndEnabled);
            if (RandomUtil.TryGetRandomElement(inactiveSentinels, out PlayerBullet bullet))
                bullet.ActivateSelf();

            #region Deprecated array implementation
            //var inactiveSentinels = SentinelPool.Where(x => !x.isActiveAndEnabled).ToArray();

            //if (RandomUtil.TryGetRandomElement(inactiveSentinels, out PlayerBullet bullet))
            //    bullet.ActivateSelf();
            #endregion Deprecated array implementation

            //timer.Stop();
            //DebugUI.SetDebugLabel(TimerName, timer.Elapsed);
        }

        public override void RunFrame(float deltaTime)
        {
            if (Level == 0)
                return;

            Rotation += deltaTime * AngularVelocity;
            if (Rotation > MathUtil.Pi2)
                Rotation -= MathUtil.Pi2;

            //if(FireTimer.UpdateActivates(deltaTime))
                //ActivateRandomSentinel();

            float angle = Rotation;
            for(int i = 0; i < NumSentinel; i++)
            {
                var sentinel = (SentinelBullet) SentinelPool[i];
                if(sentinel.isActiveAndEnabled)
                {
                    var offset = (Vector3) MathUtil.VectorAtAngle(angle, Radius);

                    var destination = transform.position + offset;
                    //var ratio = EaseIn.AdjustRatio(bullet.DistanceRatio);
                    var ratio = sentinel.RadiusRatio;
                    var pos = MathUtil.ScaledPositionBetween(transform.position, destination, ratio);
                    sentinel.transform.position = pos;
                }

                angle += AngleDelta;
            }
        }

        public void LevelUp(int level, float radius, float respawnInterval)
        {
            Level = level;

            FireTimer.ActivationInterval = respawnInterval;
            FireTimer.Reset();

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

                ActivateRandomSentinel();
            }
        }
    }
}
