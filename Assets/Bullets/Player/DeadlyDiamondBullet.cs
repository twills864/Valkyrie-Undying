using System;
using System.Linq;
using Assets.Util;
using Assets.Constants;
using Assets.GameTasks;
using Assets.ObjectPooling;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    ///
    /// </summary>
    /// <inheritdoc/>
    public class DeadlyDiamondBullet : PlayerBullet
    {
        #region Prefabs

        [SerializeField]
        private float _Speed = GameConstants.PrefabNumber;

        [SerializeField]
        private float _OffsetX = GameConstants.PrefabNumber;

        [SerializeField]
        private float _MaxAngle = GameConstants.PrefabNumber;

        #endregion Prefabs

        #region Prefab Properties

        public float Speed => _Speed;
        public float OffsetX => _OffsetX;
        public float MaxAngle => _MaxAngle;

        #endregion Prefab Properties

        protected override void OnPlayerBulletInit()
        {

        }

        protected override void OnActivate()
        {

        }

        public override void OnSpawn()
        {

        }

        protected override void OnPlayerBulletFrameRun(float deltaTime, float realDeltaTime)
        {

        }
    }
}