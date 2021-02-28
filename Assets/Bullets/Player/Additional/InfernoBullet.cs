using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class InfernoBullet : PermanentVelocityPlayerBullet
    {
        public int DamageIncreasePerTick { get; set; }

        public override void OnCollideWithEnemy(Enemy enemy)
        {
            enemy.Ignite(DamageIncreasePerTick);
            DeactivateSelf();
        }
    }
}