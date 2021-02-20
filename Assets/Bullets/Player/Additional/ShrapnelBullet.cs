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

        protected override bool ShouldMarkSelfCollision => false;

        protected override void OnActivate()
        {
            Velocity = RandomUtil.RandomDirectionVectorTopQuarter(Speed);
        }
    }
}