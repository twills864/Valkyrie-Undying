using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.FireStrategies.EnemyFireStrategies;
using Assets.UnityPrefabStructs;
using Assets.Util;
using UnityEngine;

namespace Assets.Enemies
{
    public abstract class BasicFireStrategyEnemy : Enemy
    {
        #region Prefabs

        [SerializeField]
        private VariantFireSpeed _VariantFireSpeed = default;

        #endregion Prefabs


        #region Prefab Properties

        protected VariantFireSpeed VariantFireSpeed => _VariantFireSpeed;
        protected float FireSpeed => _VariantFireSpeed.FireSpeed;
        protected float FireSpeedVariance => _VariantFireSpeed.FireSpeedVariance;

        #endregion Prefab Properties


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
