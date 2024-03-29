﻿using Assets.Constants;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// A bullet that will be fired in a spread of bullets
    /// with randomly-generated vertical and horizontal velocities.
    /// </summary>
    /// <inheritdoc/>
    public class FlakBullet : HighVelocityPlayerBullet
    {
        #region Prefabs

        [SerializeField]
        private float _BulletVelocityY = GameConstants.PrefabNumber;

        [SerializeField]
        private float _BulletOffsetX = GameConstants.PrefabNumber;
        [SerializeField]
        private float _BulletOffsetY = GameConstants.PrefabNumber;

        [SerializeField]
        private float _BulletSpreadX = GameConstants.PrefabNumber;
        [SerializeField]
        private float _BulletSpreadY = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public float BulletVelocityY => _BulletVelocityY;

        public float BulletOffsetX => _BulletOffsetX;
        public float BulletOffsetY => _BulletOffsetY;

        public float BulletSpreadX => _BulletSpreadX;
        public float BulletSpreadY => _BulletSpreadY;

        #endregion Prefab Properties


        public override AudioClip FireSound => SoundBank.ShotgunWide;
    }
}