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

        //protected override void OnPlayerBulletTriggerEnter2D(Collider2D collision)
        //{
        //    if (CollisionUtil.IsPlayerBullet(collision))
        //    {
        //        var bullet = collision.GetComponent<PlayerBullet>();
        //        if (bullet.GetType() != typeof(BurstBullet))
        //            MarkSelfCollision();
        //    }
        //}
    }
}