using Assets.Bullets.EnemyBullets;
using Assets.Constants;
using UnityEngine;

namespace Assets.EnemyBullets
{
    /// <summary>
    /// The bullet fired by the Tank enemy.
    /// This bullet travels in a straight line downward
    /// with a random x-velocity offset.
    /// </summary>
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