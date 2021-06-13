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
    /// Spawns 2 extra bullets at a default bullet's point of collision
    /// traveling at a 90 degree angle on opposite sides.
    /// </summary>
    /// <inheritdoc/>
    public class SplitPowerup : OnDefaultWeaponHitPowerup
    {
        public override int MaxLevel => 1;
        protected override Sprite GetPowerupSprite(PowerupSpriteBank bank) => bank.Split;

        private static Vector2 SpeedLeft { get; set; }
        private static Vector2 SpeedRight => SpeedLeft.ScaleX(-1f);

        protected override void InitBalance(in PowerupBalanceManager.OnDefaultWeaponHitBalance balance)
        {
            var prefab = PoolManager.Instance.BulletPool.GetPrefab<DefaultBullet>();
            float speed = prefab.InitialSpeed;
            SpeedLeft = new Vector2(-speed, 0);
        }

        public override void OnHit(Enemy enemy, DefaultBullet bullet, Vector3 hitPosition)
        {
            SplitOff(enemy, bullet, hitPosition, SpeedLeft);
            SplitOff(enemy, bullet, hitPosition, SpeedRight);
        }

        private static void SplitOff(Enemy enemy, DefaultBullet bullet, Vector3 hitPosition, Vector2 velocity)
        {
            DefaultExtraBullet.SpawnNew(hitPosition, velocity, bullet, enemy);
        }
    }
}
