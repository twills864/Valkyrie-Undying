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

        public override int Damage => RaindropDamage;
        public int RaindropDamage { get; set; }

        public override AudioClip FireSound => RandomUtil.Select(SoundBank.WaterDrip, SoundBank.WaterDrop);
        public override float FireSoundVolume => 0.4f;
    }
}