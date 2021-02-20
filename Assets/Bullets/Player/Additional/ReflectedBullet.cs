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

        public void Init(EnemyBullet source)
        {
            transform.position = source.transform.position;
            transform.localScale = source.transform.localScale;
            Velocity = -source.Velocity;
            ReflectedDamage = source.ReflectedDamage;
        }
    }
}