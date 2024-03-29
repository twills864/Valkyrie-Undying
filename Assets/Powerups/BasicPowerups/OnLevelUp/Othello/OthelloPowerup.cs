﻿using System;
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
    /// Spawns a mirror self that fires bullets alongside the player.
    /// This mirror self maintains the mirror position across the
    /// center of the screen from the player, and doesn't take damage.
    /// </summary>
    /// <inheritdoc/>
    public class OthelloPowerup : OnLevelUpPowerup
    {
        public override int MaxLevel => 2;
        protected override Sprite GetPowerupSprite(PowerupSpriteBank bank) => bank.Othello;

        public int Damage => (int)DamageCalculator.Value;
        private SumLevelValueCalculator DamageCalculator { get; set; }

        protected override void InitBalance(in PowerupBalanceManager.OnLevelUpBalance balance)
        {
            float damageBase = balance.Othello.Damage.Base;
            float damageIncrease = balance.Othello.Damage.Increase;
            DamageCalculator = new SumLevelValueCalculator(damageBase, damageIncrease);
        }


        public override void OnLevelUp()
        {
            Othello.Instance.LevelUp(Level, Damage);
        }
    }
}
