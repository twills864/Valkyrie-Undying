using Assets.Constants;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// A bullet that will be fired as a burst of bullets fired in quick succession.
    /// The number of bullets in the burst will increase with the bullet level.
    /// </summary>
    /// <inheritdoc/>
    public class BurstBullet : PlayerBullet
    {
        #region Prefabs

        [SerializeField]
        private float _BulletVelocityY = GameConstants.PrefabNumber;

        [SerializeField]
        private float _BulletSpreadX = GameConstants.PrefabNumber;
        [SerializeField]
        private float _BulletSpreadY = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public float BulletVelocityY => _BulletVelocityY;

        public float BulletSpreadX => _BulletSpreadX;
        public float BulletSpreadY => _BulletSpreadY;

        #endregion Prefab Properties


        public override AudioClip FireSound => SoundBank.LaserSmall;
    }
}