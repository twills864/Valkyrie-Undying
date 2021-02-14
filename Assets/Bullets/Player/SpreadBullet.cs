using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class SpreadBullet : PlayerBullet
    {
        public override int Damage => CurrentDamage;
        public int CurrentDamage { get; private set; }

        public void SetDamage(bool isMainBullet)
        {
            CurrentDamage = isMainBullet ? BaseDamage : AdditionalBulletDamage;
        }


        [SerializeField]
        public int AdditionalBulletDamage;
        [SerializeField]
        public float BulletVelocityY;

        [SerializeField]
        public float FireRadius;
        [SerializeField]
        public float AngleBetweenLanesInDegrees;
        [SerializeField]
        public float DampX;
    }
}