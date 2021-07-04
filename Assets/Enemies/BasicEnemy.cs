using Assets.Bullets.EnemyBullets;
using Assets.FireStrategies.EnemyFireStrategies;
using UnityEngine;

namespace Assets.Enemies
{
    /// <summary>
    /// The most basic enemy in the game, this enemy flies slowly downward
    /// and fires bullets downwards in a straight line.
    /// </summary>
    /// <inheritdoc/>
    public class BasicEnemy : PermanentVelocityEnemy
    {
        protected override EnemyFireStrategy InitialFireStrategy()
            => new BasicEnemyFireStrategy(VariantFireSpeed);

        public override AudioClip FireSound => SoundBank.GunPistol;
    }
}