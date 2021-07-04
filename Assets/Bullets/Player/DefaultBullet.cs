using System;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.Powerups;
using Assets.Powerups.DefaultBulletBuff;
using Assets.ScreenEdgeColliders;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// The default bullet fired by the player when they don't have
    /// any heavy weapons actively firing.
    /// </summary>
    /// <inheritdoc/>
    public class DefaultBullet : DefaultInfluencedBullet
    {
        public override int Damage => CalculateDamage();
        public int ExtraBulletDamage => Damage / 2;

        public override AudioClip FireSound => SoundBank.LaserBasic;

        public static void StaticInit()
        {
            ReboundActive = false;
        }

        #region Prefabs

        [SerializeField]
        private float _InitialSpeed = GameConstants.PrefabNumber;

        [SerializeField]
        private float _DefaultExtraBulletScaleRatio = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        public float InitialSpeed => _InitialSpeed;
        public float DefaultExtraBulletScaleRatio => _DefaultExtraBulletScaleRatio;

        #endregion Prefab Properties

        public static bool ReboundActive { get; set; } = false;

        protected override void OnBulletSpawn()
        {
            Velocity = CalculateVelocity(InitialSpeed);
        }

        protected override void OnDefaultInfluencedBulletCollideWithEnemy(Enemy enemy, Vector3 hitPosition)
        {
            GameManager.Instance.OnEnemyHitWithDefaultWeapon(enemy, this, hitPosition);
            DefaultBulletBuffs.OnDefaultBulletHit(this, enemy, hitPosition);
        }

        protected override void OnPlayerBulletTriggerEnter2D(Collider2D collision)
        {
            if(ReboundActive && CollisionUtil.IsScreenEdge(collision, out ScreenSide screenSide)
                && screenSide == ScreenSide.Top)
            {
                ReboundPowerup.ReboundOffScreenEdge(this);
            }
        }
    }
}