using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Randomly spawns damaging shrapnel behind a hit enemy.
    /// </summary>
    /// <inheritdoc/>
    public class ShrapnelPowerup : OnHitPowerup
    {
        private const float ShrapnelChanceBase = 0.3f;
        private const float ShrapnelIncrease = 0.8f;

        private float ShrapnelChance => ShrapnelChanceCalculator.Value;
        private AsymptoteRatioLevelValueCalculator ShrapnelChanceCalculator { get; }
            = new AsymptoteRatioLevelValueCalculator(ShrapnelChanceBase, ShrapnelIncrease);

        public float MaxY { get; set; }

        public override void OnHit(Enemy enemy, PlayerBullet bullet)
        {
            if(RandomUtil.Bool(ShrapnelChance))
            {
                var shrapnelPos = enemy.ShrapnelPosition(bullet);
                CreateShrapnel(shrapnelPos);
            }
        }

        private void CreateShrapnel(Vector2 position)
        {
            if (position.y < MaxY)
                PoolManager.Instance.BulletPool.Get<ShrapnelBullet>(position);
        }
    }
}
