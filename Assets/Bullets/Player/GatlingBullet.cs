using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class GatlingBullet : RaycastPlayerBullet
    {
        [SerializeField]
        private float LaserAlphaRatio = GameConstants.PrefabNumber;

        protected override bool ShouldMarkSelfCollision => false;
        protected override bool ShouldDeactivateOnDestructor => false;

        public override float MaxAlpha => LaserAlphaRatio;
    }
}