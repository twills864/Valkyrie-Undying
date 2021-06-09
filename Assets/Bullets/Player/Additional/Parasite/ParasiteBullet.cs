using System;
using System.Linq;
using Assets.Util;
using Assets.Constants;
using Assets.GameTasks;
using Assets.ObjectPooling;
using UnityEngine;
using Assets.Enemies;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    ///
    /// </summary>
    /// <inheritdoc/>
    public class ParasiteBullet : PlayerBullet
    {
        public override AudioClip FireSound => SoundBank.Silence;

        #region Prefabs

        [SerializeField]
        private float _RotationSpeed = GameConstants.PrefabNumber;

        [SerializeField]
        private float _SpeedMin = GameConstants.PrefabNumber;

        [SerializeField]
        private float _SpeedMax = GameConstants.PrefabNumber;

        [SerializeField]
        private float _PositionOffsetMax = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private float RotationSpeed => _RotationSpeed;
        private float SpeedMin => _SpeedMin;
        private float SpeedMax => _SpeedMax;
        public float PositionOffsetMax => _PositionOffsetMax;

        #endregion Prefab Properties


        public override void OnSpawn()
        {
            float speed = RandomUtil.Float(SpeedMin, SpeedMax, out float speedRatio);

            Vector2 initialVelocityUnit = RandomUtil.RandomDirectionVectorTopQuarter();
            Velocity = initialVelocityUnit * speed;

            Vector2 positionOffset = (speedRatio * PositionOffsetMax) * initialVelocityUnit;
            transform.position += (Vector3)positionOffset;
        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {
            RotationDegrees += (deltaTime * RotationSpeed);
        }

        protected override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            enemy.AddParasites(1);
        }
    }
}