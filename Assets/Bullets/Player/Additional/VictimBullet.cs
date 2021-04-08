﻿using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class VictimBullet : PlayerBullet
    {
        public override int Damage => base.Damage + DamageIncrease;
        public int DamageIncrease { get; set; }

        #region Prefabs

        [SerializeField]
        private float _Speed = GameConstants.PrefabNumber;

        #endregion Prefabs

        #region Prefab Properties

        public float Speed => _Speed;

        #endregion Prefab Properties
    }
}