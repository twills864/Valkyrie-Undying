using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class VictimBullet : PlayerBullet
    {
        [SerializeField]
        public float Speed;

        public override int Damage => base.Damage + DamageIncrease;
        public int DamageIncrease { get; set; }
    }
}