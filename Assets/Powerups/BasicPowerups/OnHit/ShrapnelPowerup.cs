using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.UI.SpriteBank;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Provides a chance to spawns damaging shrapnel at the point of collision when an enemy is hit.
    /// </summary>
    /// <inheritdoc/>
    public class ShrapnelPowerup : OnHitPowerup
    {
        protected override Sprite GetPowerupSprite(PowerupSpriteBank bank) => bank.Shrapnel;

        private float ShrapnelChance => ShrapnelChanceCalculator.Value;
        private SumLevelValueCalculator ShrapnelChanceCalculator { get; set; }

        protected override void InitBalance(in PowerupBalanceManager.OnHitBalance balance)
        {
            float shrapnelChanceBase = balance.Shrapnel.SpawnChance.Base;
            float shrapnelIncrease = balance.Shrapnel.SpawnChance.Increase;
            ShrapnelChanceCalculator = new SumLevelValueCalculator(shrapnelChanceBase, shrapnelIncrease);
        }

        public override void OnHit(Enemy enemy, PlayerBullet bullet, Vector3 hitPosition)
        {
            if (SpaceUtil.PointIsInBounds(hitPosition) && RandomUtil.Bool(ShrapnelChance))
            {
                float widthRatio = enemy.ColliderMap.ClampedRatioOfWidth(hitPosition.x);
                widthRatio = (widthRatio - 0.5f) * 2f;

                var shrapnel = PoolManager.Instance.BulletPool.Get<ShrapnelBullet>(hitPosition);
                shrapnel.Parent.Target = enemy;

                float speed = shrapnel.Speed;
                Vector2 velocity = new Vector2(speed * widthRatio, speed);
                velocity.Normalize();
                velocity *= speed;

                shrapnel.Velocity = velocity;

                if (enemy.IsBurning)
                {
                    shrapnel.FireDamage = enemy.BurningDamageIncrease;
                    shrapnel.FireDamageMax = enemy.BurningDamageMax;
                }

                shrapnel.OnSpawn();
            }
        }
    }
}
