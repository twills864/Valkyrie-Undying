using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// A bullet that will immediately hit the first enemy in its path
    /// on the very first frame that it's fired.
    /// It will fire with a random spread that becomes more precise
    /// with higher bullet levels.
    /// </summary>
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
        public override AudioClip FireSound => SoundBank.GunShortStrong;

        public override float MaxAlpha => LaserAlphaRatio;

        public override Vector3 GetHitPosition(Enemy enemy) => EndPoint;
    }
}