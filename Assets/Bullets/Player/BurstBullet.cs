using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class BurstBullet : PlayerBullet
    {
        [SerializeField]
        public float BulletVelocityY;

        [SerializeField]
        public float BulletSpreadX;
        [SerializeField]
        public float BulletSpreadY;
    }
}