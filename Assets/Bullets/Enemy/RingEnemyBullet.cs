using Assets.Constants;
using UnityEngine;

namespace Assets.Bullets.EnemyBullets
{
    /// <inheritdoc/>
    public class RingEnemyBullet : EnemyBullet
    {
        #region Prefabs

        [SerializeField]
        private Vector2 _Speed = Vector2.zero;

        [SerializeField]
        private float _OffsetX = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public Vector2 RightVelocity => _Speed;
        public Vector2 LeftVelocity => new Vector2(-RightVelocity.x, RightVelocity.y);
        public float OffsetX => _OffsetX;

        #endregion Prefab Properties
    }
}