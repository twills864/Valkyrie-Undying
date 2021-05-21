using System;
using System.Linq;
using Assets.Util;
using Assets.Constants;
using Assets.ObjectPooling;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    ///
    /// </summary>
    /// <inheritdoc/>
    public class MortarBullet : PlayerBullet
    {
        #region Prefabs

        [SerializeField]
        private float _Speed = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public float Speed => _Speed;

        #endregion Prefab Properties


        public override int Damage => MortarDamage;
        public int MortarDamage { get; set; }

        protected override AudioClip InitialFireSound => SoundBank.ExplosionShortestCannonFire;
    }
}