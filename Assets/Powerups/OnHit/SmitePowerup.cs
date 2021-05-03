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
    public class SmitePowerup : OnHitPowerup
    {
        public override int MaxLevel => 1;

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
            if(enemy.isActiveAndEnabled && RandomUtil.Bool(SmiteChance) && !(bullet is SmiteBullet))
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
