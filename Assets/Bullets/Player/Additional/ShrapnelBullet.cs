using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class ShrapnelBullet : PlayerBullet
    {
        [SerializeField]
        private float Speed;
        //public override bool CollidesWithEnemy(Enemy enemy)
        //{
        //    return base.CollidesWithEnemy(enemy);
        //}

        protected override void OnPlayerBulletInit()
        {
            Velocity = RandomUtil.RandomDirectionVectorTopQuarter(Speed);
        }
    }
}