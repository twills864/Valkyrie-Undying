using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class RaindropBullet : PermanentVelocityPlayerBullet
    {
        protected override bool ShouldMarkSelfCollision => false;

        public override int Damage => RaindropDamage;
        public int RaindropDamage { get; set; }

        protected override AudioClip InitialFireSound => SoundBank.Silence;
    }
}