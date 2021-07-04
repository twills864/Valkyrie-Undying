using System;
using System.Linq;
using Assets.Constants;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.EnemyBullets
{
    /// <summary>
    /// The bullet fired by the Wasp enemy.
    /// This bullet travels in a straight line directly toward the player.
    /// </summary>
    /// <inheritdoc/>
    public class WaspEnemyBullet : EnemyBullet
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