using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class SpreadBullet : PlayerBullet
    {
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