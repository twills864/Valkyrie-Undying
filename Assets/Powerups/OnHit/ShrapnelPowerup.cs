using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Enemies;
using Assets.Util;

namespace Assets.Powerups
{
    public class ShrapnelPowerup : OnHitPowerup
    {
        private const float ShrapnelChanceBase = 0.2f;
        private const float ShrapnelIncrease = 0.1f;

        protected override LeveledValueCalculator DefaultValueCalculator
            => new LeveledValueCalculator(ShrapnelChanceBase, ShrapnelIncrease);

        private float ShrapnelChance => ValueCalculator.Value;

        public override void OnHit(Enemy enemy)
        {
            if(RandomUtil.Bool(ShrapnelChance))
            {
                var shrapnelPos = enemy.RandomShrapnelPosition();
                GameManager.Instance.CreateShrapnel(shrapnelPos);
            }
        }
    }
}
