using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.FireStrategies.EnemyFireStrategies;
using Assets.Util;
using UnityEngine;

namespace Assets.Enemies
{
    public abstract class FireStrategyEnemy : Enemy
    {
        [SerializeField]
        protected float FireSpeed;

        [SerializeField]
        protected float FireSpeedVariance;

        public LoopingFrameTimer FireTimer => FireStrategy.FireTimer;

        protected virtual void OnFireStrategyEnemyActivate() { }
        protected sealed override void OnEnemyActivate()
        {
            FireStrategy.Reset();
            OnFireStrategyEnemyActivate();
        }

        protected virtual void OnFireStrategyEnemyFrame(float deltaTime, float realDeltaTime) { }
        protected sealed override void OnEnemyFrame(float deltaTime, float realDeltaTime)
        {
            if (FireTimer.UpdateActivates(deltaTime))
                FireBullets();

            OnFireStrategyEnemyFrame(deltaTime, realDeltaTime);
        }

        protected virtual void FireBullets()
        {
            if (CanFire(FirePosition))
            {
                var bullets = FireStrategy.GetBullets(FirePosition);
            }
        }
    }
}
