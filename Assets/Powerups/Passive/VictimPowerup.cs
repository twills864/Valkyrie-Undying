using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets.Powerups
{
    /// <summary>
    /// The player can click an enemy that will be the victim of homing bullets.
    /// </summary>
    /// <inheritdoc/>
    public class VictimPowerup : PassivePowerup
    {
        private const float FireSpeedBase = 0.5f;
        private const float FireSpeedScale = 0.9f;

        protected override LevelValueCalculator InitialValueCalculator
            => new ProductLevelValueCalculator(FireSpeedBase, FireSpeedScale);

        public override void RunFrame(float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
