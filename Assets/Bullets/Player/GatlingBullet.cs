using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class GatlingBullet : RaycastPlayerBullet
    {
        public static float LaserAlphaRatio { get; set; }

        protected override bool ShouldMarkSelfCollision => false;
        protected override bool ShouldDeactivateOnDestructor => false;

        public override float MaxAlpha => LaserAlphaRatio;
    }
}