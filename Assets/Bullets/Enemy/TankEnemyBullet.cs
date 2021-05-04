using Assets.Bullets.EnemyBullets;
using Assets.Constants;
using UnityEngine;

namespace Assets.EnemyBullets
{
    /// <inheritdoc/>
    public class TankEnemyBullet : EnemyBullet
    {
        #region Prefabs

        [SerializeField]
        private float _Speed = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public float Speed => _Speed;

        #endregion Prefab Properties
    }
}