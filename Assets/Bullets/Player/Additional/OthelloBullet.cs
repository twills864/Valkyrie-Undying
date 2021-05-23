using Assets.Bullets.PlayerBullets;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class OthelloBullet : PermanentVelocityPlayerBullet
    {
        public override int Damage => OthelloDamage;
        public int OthelloDamage { get; set; }

        public override AudioClip FireSound => SoundBank.Silence;
    }
}