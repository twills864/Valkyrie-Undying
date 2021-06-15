using System;
using System.Linq;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.UI.SpriteBank;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Piercing Rounds causes default bullets to penetrate enemies.
    /// Level 1 - One enemy can be penetrated.
    /// Level 2 - Two enemies can be penetrated, and extra bullets
    ///           can also penetrate one enemy.
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
            DefaultBullet.MaxPenetration = 1;
            //DefaultExtraBullet.MaxPenetration = 1;

            //if (Level == 1)
            //DefaultBullet.MaxPenetration = 1;
            //else
            //{
            //    DefaultBullet.MaxPenetration = 2;
            //    DefaultExtraBullet.MaxPenetration = 1;
            //}
        }
    }
}
