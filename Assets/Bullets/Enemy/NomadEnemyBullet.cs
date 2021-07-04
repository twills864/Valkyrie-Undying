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
    /// The bullet fired by the Nomad enemy.
    /// This bullet travels straight down.
    /// </summary>
    /// <inheritdoc/>
    public class NomadEnemyBullet : PermanentVelocityEnemyBullet
    {
        #region Prefabs

        [SerializeField]
        private float _SpawnOffsetX = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public float SpawnOffsetX => _SpawnOffsetX;

        #endregion Prefab Properties



    }
}