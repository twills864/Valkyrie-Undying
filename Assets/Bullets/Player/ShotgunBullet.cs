using Assets.Constants;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// A bullet that fires as part of a spread of bullets fired in a rough cone shape.
    /// The number of bullets in the spread increases with the bullet level.
    /// </summary>
    /// <inheritdoc/>
    public class ShotgunBullet : HighVelocityPlayerBullet
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


        public override AudioClip FireSound => SoundBank.ExplosionShortestHard;
    }
}