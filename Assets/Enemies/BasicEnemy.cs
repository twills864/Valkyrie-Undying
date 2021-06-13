using Assets.Bullets.EnemyBullets;
using Assets.FireStrategies.EnemyFireStrategies;
using UnityEngine;

namespace Assets.Enemies
{
    /// <inheritdoc/>
    public class BasicEnemy : PermanentVelocityEnemy
    {
        protected override EnemyFireStrategy InitialFireStrategy()
            => new BasicEnemyFireStrategy(VariantFireSpeed);

        public override AudioClip FireSound => SoundBank.GunPistol;
    }
}