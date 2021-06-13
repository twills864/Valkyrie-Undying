using System;
using System.Linq;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.UI.SpriteBank;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Spawns extra bullets at a default bullet's point of collision
    /// traveling at a 45 degree angle.
    /// Level 1 - One bullet on a random side.
    /// Level 2 - Two bullets total, one on each side.
    /// </summary>
    /// <inheritdoc/>
    public class SplinterPowerup : OnDefaultWeaponHitPowerup
    {
        public override int MaxLevel => 1;
        protected override Sprite GetPowerupSprite(PowerupSpriteBank bank) => bank.Splinter;

        private static Vector2 SpeedLeft { get; set; }
        private static Vector2 SpeedRight => SpeedLeft.ScaleX(-1f);

        protected override void InitBalance(in PowerupBalanceManager.OnDefaultWeaponHitBalance balance)
        {
            float angleOffset = balance.Splinter.AngleInDegrees * Mathf.Deg2Rad;

            var prefab = PoolManager.Instance.BulletPool.GetPrefab<DefaultBullet>();
            float speed = prefab.InitialSpeed;

            const float AngleUp = MathUtil.PiHalf;
            float angle = AngleUp + angleOffset;
            SpeedLeft = MathUtil.Vector2AtRadianAngle(angle, speed);
        }

        public override void OnHit(Enemy enemy, DefaultBullet bullet, Vector3 hitPosition)
        {
            if(Level == 1)
            {
                Vector2 velocity = RandomUtil.Select(SpeedLeft, SpeedRight);
                SplinterOff(enemy, bullet, hitPosition, velocity);
            }
            else
            {
                SplinterOff(enemy, bullet, hitPosition, SpeedLeft);
                SplinterOff(enemy, bullet, hitPosition, SpeedRight);
            }
        }

        private static void SplinterOff(Enemy enemy, DefaultBullet bullet, Vector3 hitPosition, Vector2 velocity)
        {
            DefaultExtraBullet.SpawnNew(hitPosition, velocity, bullet, enemy);
        }
    }
}
