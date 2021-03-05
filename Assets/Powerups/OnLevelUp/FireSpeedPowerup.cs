using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets.Powerups
{
    /// <summary>
    /// Increases the rate at which the player fires their main cannon.
    /// </summary>
    /// <inheritdoc/>
    public class FireSpeedPowerup : OnLevelUpPowerup
    {
        private const float FireDeltaBase = 1.2f;
        private const float FireDeltaIncrease = 0.15f;

        private float PlayerFireDeltaTimeScale => PlayerFireDeltaTimeCalculator.Value;
        private SumLevelValueCalculator PlayerFireDeltaTimeCalculator { get; }
            = new SumLevelValueCalculator(FireDeltaBase, FireDeltaIncrease);

        public override void OnLevelUp()
        {
            GameManager.Instance.PlayerFireDeltaTimeScale = PlayerFireDeltaTimeScale;
        }
    }
}
