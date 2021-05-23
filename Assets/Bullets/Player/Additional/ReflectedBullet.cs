using Assets.Bullets.EnemyBullets;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class ReflectedBullet : PlayerBullet
    {
        protected override bool ShouldMarkSelfCollision => false;

        private int ReflectedDamage { get; set; }
        public override int Damage => ReflectedDamage;

        public override AudioClip FireSound => SoundBank.Silence;

        private void Init(EnemyBullet source)
        {
            transform.position = source.transform.position;
            transform.localScale = source.transform.localScale;
            ReflectedDamage = source.ReflectDamage;
        }

        // Adopt velocity from Pest Control
        public void RedirectFromPestControl(EnemyBullet source, PestControlBullet pestControl)
        {
            Init(source);

            const float velocityScale = 0.75f;
            Velocity = pestControl.Velocity * velocityScale;

        }

        // Reflect directly upwards
        public void ReflectBack(EnemyBullet source)
        {
            Init(source);

            const float velocityScale = 2f;
            Velocity = -velocityScale * source.Velocity;
        }
    }
}