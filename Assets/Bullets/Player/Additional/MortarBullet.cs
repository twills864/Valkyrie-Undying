using System;
using System.Linq;
using Assets.Util;
using Assets.Constants;
using Assets.ObjectPooling;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// The bullet fired from the bottom corners of the screen as part of the Mortar powerup.
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

        public override AudioClip FireSound => SoundBank.ExplosionShortestCannonFire;
        public override float FireSoundVolume => 0.4f;
    }
}