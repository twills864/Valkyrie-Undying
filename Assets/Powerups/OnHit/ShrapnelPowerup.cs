using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.Util;

namespace Assets.Powerups
{
    /// <summary>
    /// Randomly spawns damaging shrapnel behind a hit enemy.
    /// </summary>
    /// <inheritdoc/>
    public class ShrapnelPowerup : OnHitPowerup
    {
        private const float ShrapnelChanceBase = 0.2f;
        private const float ShrapnelIncrease = 0.1f;

        protected override LevelValueCalculator InitialValueCalculator
            => new SumLevelValueCalculator(ShrapnelChanceBase, ShrapnelIncrease);

        private float ShrapnelChance => ValueCalculator.Value;

        public override void OnHit(Enemy enemy, PlayerBullet bullet)
        {
            if(RandomUtil.Bool(ShrapnelChance))
            {
                var shrapnelPos = enemy.ShrapnelPosition(bullet);
                GameManager.Instance.CreateShrapnel(shrapnelPos);
            }
        }
    }
}
