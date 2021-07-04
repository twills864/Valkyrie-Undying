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
    /// Provides a small chance to spawn a powerful lightning bolt that will
    /// target the enemy that was hit.
    ///
    /// The lightning bolt is strong enough to instantly kill enemies in the
    /// early game, but deals a finite amount of damage to balance the late game.
    /// </summary>
    /// <inheritdoc/>
    public class SmitePowerup : OnHitPowerup
    {
        public override int MaxLevel => 1;
        protected override Sprite GetPowerupSprite(PowerupSpriteBank bank) => bank.Smite;

        private float SmiteChance => ChanceCalculator.Value;
        private SumLevelValueCalculator ChanceCalculator { get; set; }

        protected override void InitBalance(in PowerupBalanceManager.OnHitBalance balance)
        {
            float chanceBase = balance.Smite.Chance.Base;
            float chanceIncrease = balance.Smite.Chance.Increase;
            ChanceCalculator = new SumLevelValueCalculator(chanceBase, chanceIncrease);
        }

        public override void OnLevelUp()
        {
            if (Director.TryGetRandomEnemy(out Enemy enemy))
                SmiteEnemy(enemy);
        }

        public override void OnHit(Enemy enemy, PlayerBullet bullet, Vector3 hitPosition)
        {
            if(RandomUtil.Bool(SmiteChance) && !(bullet is SmiteBullet))
            {
                SmiteEnemy(enemy);
            }
        }

        private void SmiteEnemy(Enemy enemy)
        {
            float x = SpaceUtil.RandomWorldPositionX();
            float y = SpaceUtil.WorldMap.Bottom.y;
            Vector3 startPosition = new Vector3(x, y);

            SmiteJointBullet.StartSmite(startPosition, enemy);
        }
    }
}
