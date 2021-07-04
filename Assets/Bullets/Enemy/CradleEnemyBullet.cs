using Assets.Constants;
using UnityEngine;

namespace Assets.Bullets.EnemyBullets
{
    /// <summary>
    /// The bullet fired by the Cradle enemy.
    /// This bullet travels in a straight line, either straight down,
    /// diagonally down-left, or diagonally down-right depending on
    /// the state the Cradle enemy was in when it was fired.
    /// </summary>
    /// <inheritdoc/>
    public class CradleEnemyBullet : EnemyBullet
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