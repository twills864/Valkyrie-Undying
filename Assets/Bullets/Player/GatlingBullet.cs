using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public class GatlingBullet : RaycastPlayerBullet
    {
        #region Prefabs

        [SerializeField]
        private float _LaserAlphaRatio = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private float LaserAlphaRatio => _LaserAlphaRatio;

        #endregion Prefab Properties


        protected override bool ShouldMarkSelfCollision => false;
        protected override bool ShouldDeactivateOnDestructor => false;
        protected override AudioClip InitialFireSound => SoundBank.GunShortStrong;

        public override float MaxAlpha => LaserAlphaRatio;

        public override Vector3 GetHitPosition(Enemy enemy) => EndPoint;
    }
}