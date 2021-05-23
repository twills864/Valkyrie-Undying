using System;
using Assets.EnemyBullets;
using Assets.FireStrategies;
using Assets.FireStrategies.EnemyFireStrategies;
using Assets.UnityPrefabStructs;
using UnityEngine;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public class TankEnemy : PermanentVelocityEnemy
    {
        #region Prefabs

        [SerializeField]
        private TankVariantFireSpeedExtra _FireSpeedExtra = default;

        #endregion Prefabs


        #region Prefab Properties

        public TankVariantFireSpeedExtra FireSpeedExtra => _FireSpeedExtra;

        #endregion Prefab Properties

        protected override EnemyFireStrategy InitialFireStrategy()
            => new TankEnemyFireStrategy(VariantFireSpeed, FireSpeedExtra);

        public override AudioClip FireSound => SoundBank.ExplosionShortestDeepTacticalFire;
        public override float FireSoundVolume => 0.5f;

        [Serializable]
        public struct TankVariantFireSpeedExtra
        {
            public float ReloadSpeed;

            public int NumBulletsPerBurst;
            public float SpeedVariancePerBullet;
        }
    }
}