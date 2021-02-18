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
    }
}