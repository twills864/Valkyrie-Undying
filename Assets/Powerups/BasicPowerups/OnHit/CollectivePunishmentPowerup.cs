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
    /// Damages every living enemy on the screen any time any enemy is hit.
    /// </summary>
    /// <inheritdoc/>
    public class CollectivePunishmentPowerup : OnHitPowerup
    {
        public override int MaxLevel => 1;
        protected override Sprite GetPowerupSprite(PowerupSpriteBank bank) => bank.CollectivePunishment;

        private float PowerValue => PowerCalculator.Value;
        private SumLevelValueCalculator PowerCalculator { get; set; }

        protected override void InitBalance(in PowerupBalanceManager.OnHitBalance balance)
        {
            float powerBase = balance.CollectivePunishment.Power.Base;
            float powerIncrease = balance.CollectivePunishment.Power.Increase;
            PowerCalculator = new SumLevelValueCalculator(powerBase, powerIncrease);
        }

        public override void OnHit(Enemy enemy, PlayerBullet bullet, Vector3 hitPosition)
        {
            int damage = (int)PowerValue;
            enemy.TrueDamage(damage);

            var allEnemies = Director.GetAllActiveEnemies();
            foreach (var target in allEnemies.Where(x => x != enemy))
            {
                target.CollectivePunishmentParticleEffect(Level);
                target.TrueDamage(damage);
            }
        }
    }
}
