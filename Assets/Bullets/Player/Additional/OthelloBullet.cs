using Assets.Bullets.PlayerBullets;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class OthelloBullet : PermanentVelocityPlayerBullet
    {
        public override int Damage => OthelloDamage;
        public int OthelloDamage { get; set; }

        protected override AudioClip InitialFireSound => SoundBank.Silence;
    }
}