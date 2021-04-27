using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
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
        protected override void InitBalance(in PowerupBalanceManager.OnHitBalance balance)
        {
            float shrapnelChanceBase = balance.Shrapnel.SpawnChance.Base;
            float shrapnelIncrease = balance.Shrapnel.SpawnChance.Increase;
            ShrapnelChanceCalculator = new SumLevelValueCalculator(shrapnelChanceBase, shrapnelIncrease);
        }

        private float ShrapnelChance => ShrapnelChanceCalculator.Value;
        private SumLevelValueCalculator ShrapnelChanceCalculator { get; set; }

        //public float MaxY { get; set; }

        public override void OnHit(Enemy enemy, PlayerBullet bullet, Vector3 hitPosition)
        {
            if (SpaceUtil.WorldMap.ContainsPoint(hitPosition) && RandomUtil.Bool(ShrapnelChance))
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

                if(enemy.IsBurning)
                    shrapnel.FireDamage = enemy.InfernoDamageIncrease;
            }
        }
    }
}
