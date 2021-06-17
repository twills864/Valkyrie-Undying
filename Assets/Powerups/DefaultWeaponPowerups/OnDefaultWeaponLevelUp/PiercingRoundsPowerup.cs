using System;
using System.Linq;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.Powerups.DefaultBulletBuff;
using Assets.UI.SpriteBank;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Piercing Rounds causes default bullets to penetrate enemies.
    /// Level 1 - One enemy can be penetrated. Extra bullets also receive this effect.
    /// </summary>
    /// <inheritdoc/>
    public class PiercingRoundsPowerup : OnDefaultWeaponLevelUpPowerup
    {
        public override int MaxLevel => 1;
        protected override Sprite GetPowerupSprite(PowerupSpriteBank bank) => bank.PiercingRounds;

        protected override void InitBalance(in PowerupBalanceManager.OnDefaultWeaponLevelUpBalance balance)
        {
            // No action necessary.
        }

        public override void OnLevelUp()
        {
            DefaultBulletBuffs.PiercingRoundsLevelup(this);
        }
    }
}
