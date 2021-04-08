using Assets.Constants;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
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



    }
}