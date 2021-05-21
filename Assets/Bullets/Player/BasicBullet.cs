using Assets.Bullets.PlayerBullets;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class BasicBullet : PermanentVelocityPlayerBullet
    {
        protected override AudioClip InitialFireSound => SoundBank.LaserGeneric;
    }
}