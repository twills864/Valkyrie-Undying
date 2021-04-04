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
        public override int Damage => MortarDamage;

        [SerializeField]
        public float Speed = GameConstants.PrefabNumber;

        public int MortarDamage { get; set; }
    }
}