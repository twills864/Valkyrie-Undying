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
        [SerializeField]
        private float _Speed = GameConstants.PrefabNumber;
        public float Speed => _Speed;

        [SerializeField]
        private float _OffsetX = GameConstants.PrefabNumber;
        public float OffsetX => _OffsetX;

        [SerializeField]
        private float _MaxAngle = GameConstants.PrefabNumber;
        public float MaxAngle => _MaxAngle;

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