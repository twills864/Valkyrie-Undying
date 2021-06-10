using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Powerups.Balance;
using Assets.UI.SpriteBank;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Increases the rate at which the player fires their main cannon.
    /// </summary>
    /// <inheritdoc/>
    public class FireSpeedPowerup : OnLevelUpPowerup
    {
        protected override Sprite GetPowerupSprite(PowerupSpriteBank bank) => bank.FireSpeed;

        private float PlayerFireDeltaTimeScale => PlayerFireDeltaTimeCalculator.Value;
        private SumLevelValueCalculator PlayerFireDeltaTimeCalculator { get; set; }

        protected override void InitBalance(in PowerupBalanceManager.OnLevelUpBalance balance)
        {
            float fireDeltaBase = balance.FireSpeed.Base;
            float fireDeltaIncrease = balance.FireSpeed.Increase;
            PlayerFireDeltaTimeCalculator = new SumLevelValueCalculator(fireDeltaBase, fireDeltaIncrease);
        }

        public override void OnLevelUp()
        {
            GameManager.Instance.PlayerFireDeltaTimeScale = PlayerFireDeltaTimeScale;
        }
    }
}
