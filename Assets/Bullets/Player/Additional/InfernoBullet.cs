using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.Powerups;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// The bullet fired periodically when the player has the Inferno powerup.
    /// This bullet adds burning damage to any enemy hit.
    /// </summary>
    /// <inheritdoc/>
    public class InfernoBullet : PermanentVelocityPlayerBullet
    {
        protected override bool AutomaticallyDeactivate => false;
        public override AudioClip FireSound => SoundBank.Flare;
        public override AudioClip HitSound => SoundBank.ExplosionShortestIgnite;

        public int DamageIncreasePerTick { get; set; }
        public int MaxDamage { get; set; }

        public override int Damage => InfernoDamage;
        private int InfernoDamage { get; set; }

        protected override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            if(enemy.isActiveAndEnabled)
                enemy.Ignite(Damage, DamageIncreasePerTick, MaxDamage);

            if(BulletLevel <= 1)
                DeactivateSelf();
        }

        public void OnSpawn(InfernoPowerup powerup, Vector3 firePos)
        {
            transform.position = firePos;
            BulletLevel = powerup.Level;
            InfernoDamage = powerup.Level;
            DamageIncreasePerTick = powerup.DamageIncrease;
            MaxDamage = powerup.MaxDamage;

            PlayFireSound();
        }
    }
}