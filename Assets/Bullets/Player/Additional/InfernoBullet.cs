using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class InfernoBullet : PermanentVelocityPlayerBullet
    {
        public static int CurrentBaseDamage { get; set; }

        public int DamageIncreasePerTick { get; set; }

        protected override void OnActivate()
        {
            CurrentBaseDamage = BaseDamage;
        }

        public override void OnCollideWithEnemy(Enemy enemy)
        {
            enemy.Ignite(Damage, DamageIncreasePerTick);
            DeactivateSelf();
        }
    }
}