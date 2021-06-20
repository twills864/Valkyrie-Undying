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
    ///
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