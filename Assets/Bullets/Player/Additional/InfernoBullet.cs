using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class InfernoBullet : PermanentVelocityPlayerBullet
    {
        public sealed override AudioClip FireSound => SoundBank.Flare;
        public sealed override AudioClip HitSound => SoundBank.ExplosionShortestIgnite;

        public static int CurrentBaseDamage { get; set; }

        public int DamageIncreasePerTick { get; set; }

        protected override void OnActivate()
        {
            CurrentBaseDamage = BaseDamage;
        }

        public override void OnCollideWithEnemy(Enemy enemy)
        {
            if(enemy.isActiveAndEnabled)
                enemy.Ignite(Damage, DamageIncreasePerTick);

            DeactivateSelf();
        }
    }
}