using System;
using System.Linq;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    ///
    /// </summary>
    /// <inheritdoc/>
    public class ReboundPowerup : OnDefaultWeaponHitPowerup
    {
        public override int MaxLevel => 1;

        private Vector2 SpeedLeft { get; set; }
        private Vector2 SpeedRight => VectorUtil.ScaleX2(SpeedLeft, -1f);

        protected override void InitBalance(in PowerupBalanceManager.OnDefaultWeaponHitBalance balance)
        {
            float angleOffset = balance.Rebound.AngleInDegrees * Mathf.Deg2Rad;

            var prefab = PoolManager.Instance.BulletPool.GetPrefab<DefaultBullet>();
            float speed = prefab.InitialSpeed;

            const float AngleDown = 1.5f * Mathf.PI;
            float angle = AngleDown - angleOffset;
            SpeedLeft = MathUtil.Vector2AtRadianAngle(angle, speed);
        }

        public override void OnHit(Enemy enemy, DefaultBullet bullet, Vector3 hitPosition)
        {
            DefaultExtraBullet.SpawnNew(hitPosition, SpeedLeft, bullet, enemy);
            DefaultExtraBullet.SpawnNew(hitPosition, SpeedRight, bullet, enemy);
        }
    }
}
