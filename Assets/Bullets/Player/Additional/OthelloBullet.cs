using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.Powerups.DefaultBulletBuff;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class OthelloBullet : PermanentVelocityPlayerBullet
    {
        public override int Damage => OthelloDamage;
        public int OthelloDamage { get; set; }

        public override AudioClip FireSound => SoundBank.Silence;

        protected override void OnCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            DefaultBulletBuffs.OnOthelloBulletHit(this, enemy, hitPosition);
        }
    }
}