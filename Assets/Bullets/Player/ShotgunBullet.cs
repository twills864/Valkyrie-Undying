using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class ShotgunBullet : ConstantVelocityBullet
    {
        [SerializeField]
        public float BulletOffsetX;
        [SerializeField]
        public float BulletOffsetY;
        [SerializeField]
        public float BulletSpreadX;
        [SerializeField]
        public float BulletSpreadY;
    }
}