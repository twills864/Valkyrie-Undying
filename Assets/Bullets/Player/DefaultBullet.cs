using Assets.Bullets.PlayerBullets;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class DefaultBullet : PermanentVelocityPlayerBullet
    {
        public virtual bool OverrideEnemyVelocityOnKill => false;
        public override AudioClip FireSound => SoundBank.LaserBasic;
    }
}