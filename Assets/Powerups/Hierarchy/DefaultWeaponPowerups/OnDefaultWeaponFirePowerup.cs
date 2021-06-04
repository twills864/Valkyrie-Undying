using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets;
using Assets.Bullets.PlayerBullets;
using Assets.Powerups.Balance;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Represents a powerup that will be applied when the player fires
    /// the default bullet from their main cannon.
    /// </summary>
    /// <inheritdoc/>
    public abstract class OnDefaultWeaponFirePowerup : DefaultWeaponPowerup
    {
        public override void OnLevelUp() { }
        public abstract void OnFire(Vector3 position, DefaultBullet[] bullets);

        protected sealed override void InitBalance(in PowerupBalanceManager balance)
            => InitBalance(in balance.OnDefaultWeaponFire);

        protected abstract void InitBalance(in PowerupBalanceManager.OnDefaultWeaponFireBalance balance);
    }
}
