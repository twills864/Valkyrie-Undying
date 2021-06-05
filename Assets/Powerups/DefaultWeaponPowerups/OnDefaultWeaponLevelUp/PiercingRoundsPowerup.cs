using System;
using System.Linq;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Piercing Rounds causes default bullets to penetrate enemies.
    /// Level 1 - One enemy can be penetrated.
    /// Level 2 - An unlimited number of enemies can be penetrated.
    /// </summary>
    /// <inheritdoc/>
    public class PiercingRoundsPowerup : OnDefaultWeaponLevelUpPowerup
    {
        public override int MaxLevel => 2;

        protected override void InitBalance(in PowerupBalanceManager.OnDefaultWeaponLevelUpBalance balance)
        {
            // No action necessary.
        }

        public override void OnLevelUp()
        {
            if (Level == 1)
                DefaultBullet.MaxPenetration = 1;
            else
                DefaultBullet.MaxPenetration = int.MaxValue;
        }
    }
}
