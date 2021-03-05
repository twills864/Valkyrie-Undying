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

        private const float DamageBase = 0;
        private const float DamageIncrease = 10;

        public float FireSpeedModifier => FireSpeedModifierCalculator.Value;
        private AsymptoteScaleLevelValueCalculator FireSpeedModifierCalculator { get; set; }
            = new AsymptoteScaleLevelValueCalculator(FireSpeedExponentBase);

        public int Damage => (int)DamageCalculator.Value;
        private SumLevelValueCalculator DamageCalculator { get; set; }
            = new SumLevelValueCalculator(DamageBase, DamageIncrease);

        public override void OnLevelUp()
        {
            Othello.Instance.LevelUp(Level, Damage, FireSpeedModifier);
        }
    }
}
