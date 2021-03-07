using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Powerups.Balance;
using Assets.Util;

namespace Assets.Powerups
{
    /// <summary>
    /// Increases the rate at which the player fires their main cannon.
    /// </summary>
    /// <inheritdoc/>
    public class FireSpeedPowerup : OnLevelUpPowerup
    {
        protected override void InitBalance(in PowerupBalanceManager.OnLevelUpBalance balance)
        {
            float fireDeltaBase = balance.Firespeed.Base;
            float fireDeltaIncrease = balance.Firespeed.Increase;
            PlayerFireDeltaTimeCalculator = new SumLevelValueCalculator(fireDeltaBase, fireDeltaIncrease);
        }

        //private const float FireDeltaBase = 1.2f;
        //private const float FireDeltaIncrease = 0.15f;

        private float PlayerFireDeltaTimeScale => PlayerFireDeltaTimeCalculator.Value;
        private SumLevelValueCalculator PlayerFireDeltaTimeCalculator { get; set; }

        public override void OnLevelUp()
        {
            GameManager.Instance.PlayerFireDeltaTimeScale = PlayerFireDeltaTimeScale;
        }
    }
}
