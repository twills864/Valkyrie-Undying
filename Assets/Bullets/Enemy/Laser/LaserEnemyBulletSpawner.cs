using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.FireStrategies.PlayerFireStrategies;
using Assets.GameTasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.EnemyBullets
{
    public class LaserEnemyBulletSpawner : EnemyBullet
    {
        [SerializeField]
        private float FadeInTime = GameConstants.PrefabNumber;

        [SerializeField]
        private float FullBrightTime = GameConstants.PrefabNumber;

        private FadeTo FadeIn { get; set; }

        public bool ReadyToDeactivate => DeactivateTimer.Activated;
        private FrameTimer DeactivateTimer { get; set; }

        public float WidthHalf { get; private set; }

        public override int ReflectedDamage => throw new NotImplementedException();

        protected override void OnEnemyBulletInit()
        {
            float maxAlpha = Alpha;
            Alpha = 0f;

            FadeIn = new FadeTo(this, maxAlpha, FadeInTime);
            DeactivateTimer = new FrameTimer(FadeInTime + FullBrightTime);
        }

        protected override void OnActivate()
        {
            Alpha = 0f;
            FadeIn.ResetSelf();
            DeactivateTimer.Reset();
        }

        protected override void OnFrameRun(float deltaTime, float realDeltaTime)
        {
            DebugUI.SetDebugLabel("Spawner pos", transform.position);
            FadeIn.RunFrame(deltaTime);
            DeactivateTimer.Increment(deltaTime);
        }
    }
}
