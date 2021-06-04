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
            float fireDeltaBase = balance.FireSpeed.Base;
            float fireDeltaIncrease = balance.FireSpeed.Increase;
            PlayerFireDeltaTimeCalculator = new SumLevelValueCalculator(fireDeltaBase, fireDeltaIncrease);
        }

        private float PlayerFireDeltaTimeScale => PlayerFireDeltaTimeCalculator.Value;
        private SumLevelValueCalculator PlayerFireDeltaTimeCalculator { get; set; }

        public override void OnLevelUp()
        {
            GameManager.Instance.PlayerFireDeltaTimeScale = PlayerFireDeltaTimeScale;
        }
    }
}
