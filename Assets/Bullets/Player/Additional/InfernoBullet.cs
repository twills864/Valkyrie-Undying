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
        public int MaxDamage { get; set; }

        protected override void OnActivate()
        {
            CurrentBaseDamage = BaseDamage;
        }

        public override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            PlayHitSound();

            if(enemy.isActiveAndEnabled)
                enemy.Ignite(Damage, DamageIncreasePerTick, MaxDamage);

            if(BulletLevel <= 1)
                DeactivateSelf();
        }
    }
}