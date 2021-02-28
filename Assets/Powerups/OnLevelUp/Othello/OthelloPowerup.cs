using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets.Powerups
{
    /// <summary>
    /// Spawns a passive rain cloud behind the player that rains bullets on enemies.
    /// </summary>
    /// <inheritdoc/>
    public class OthelloPowerup : OnLevelUpPowerup
    {
        private const float FireSpeedExponentBase = 0.8f;

        protected override LevelValueCalculator InitialValueCalculator
            => new AsymptoteScaleLevelValueCalculator(FireSpeedExponentBase);

        public float FireSpeedModifier => ValueCalculator.Value;

        private const float DamageBase = 0;
        private const float DamageIncrease = 10;
        private SumLevelValueCalculator DamageCalculator =
            new SumLevelValueCalculator(DamageBase, DamageIncrease);

        public int Damage => (int)DamageCalculator.Value;

        public override void OnLevelUp()
        {
            DamageCalculator.Level = Level;
            Othello.Instance.LevelUp(Level, Damage, FireSpeedModifier);
        }
    }
}
