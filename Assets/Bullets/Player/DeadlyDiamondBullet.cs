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
    /// A bullet that will be fired a spread with an oscillating number of bullets.
    /// The maximum number of bullets in the spread increases with the bullet level.
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

        public override AudioClip FireSound => SoundBank.LaserBrief;

        private float _BulletTrailWidth { get; set; }
        public override float BulletTrailWidth => _BulletTrailWidth;

        protected override void OnPlayerBulletInit()
        {
            // Sprite is rotated 90 degrees in source.
            _BulletTrailWidth = ColliderMap.Height;
        }
    }
}