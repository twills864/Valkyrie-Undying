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
        const int NumSentinel = 8;
        readonly float AngleDelta = MathUtil.Pi2 / NumSentinel;

        [SerializeField]
        private float AngularVelocity;

        private LoopingFrameTimer FireTimer = LoopingFrameTimer.Default();

        private int Level { get; set; }
        private float Distance { get; set; }
        private float Rotation { get; set; }
        //private float RespawnInterval { get; set; }

        private ObjectPool<PlayerBullet> SentinelPool;

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
            var inactiveSentinels = SentinelPool.Where(x => !x.isActiveAndEnabled);
            if (RandomUtil.TryGetRandomElement(inactiveSentinels, out PlayerBullet bullet))
                bullet.ActivateSelf();


            // Array implmentation
            //var inactiveSentinels = SentinelPool.Where(x => !x.isActiveAndEnabled).ToArray();

            //if (RandomUtil.TryGetRandomElement(inactiveSentinels, out PlayerBullet bullet))
            //    bullet.ActivateSelf();

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

            if(FireTimer.UpdateActivates(deltaTime))
                ActivateRandomSentinel();

            float angle = Rotation;
            for(int i = 0; i < NumSentinel; i++)
            {
                var bullet = (SentinelBullet) SentinelPool[i];
                if(bullet.isActiveAndEnabled)
                {
                    var offset = (Vector3) MathUtil.VectorAtAngle(angle, Distance);

                    var destination = transform.position + offset;
                    //var ratio = EaseIn.AdjustRatio(bullet.DistanceRatio);
                    var ratio = bullet.DistanceRatio;
                    var pos = MathUtil.ScaledPositionBetween(transform.position, destination, ratio);
                    bullet.transform.position = pos;
                }

                angle += AngleDelta;
            }
        }

        public void LevelUp(int level, float distance, float respawnInterval)
        {
            Level = level;

            FireTimer.ActivationInterval = respawnInterval;
            FireTimer.Reset();

            float oldDistance = Distance;
            Distance = distance;

            if (level > 1)
            {
                float newDistanceRatio = oldDistance / Distance;

                foreach (var bullet in SentinelPool.Where(x => x.isActiveAndEnabled))
                {
                    var sentinel = (SentinelBullet)bullet;
                    sentinel.UpdateTimerRatio(newDistanceRatio);


                    // Old musings - may come back
                    //// newRatio = Inverse(IDFNew(oldPosition))
                    //var oldInverse = scale.FindInverseOfValueAsRatio(oldDistance);
                    //var newRatio = EaseIn.InverseRatio(oldInverse);

                    ////var oldRatio = sentinel.DistanceRatio;
                    ////var inverse = EaseIn.InverseRatio(oldRatio);
                }
            }

            ActivateRandomSentinel();
        }
    }
}
