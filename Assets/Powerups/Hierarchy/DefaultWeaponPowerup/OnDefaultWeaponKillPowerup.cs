using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.Powerups.Balance;

namespace Assets.Powerups
{
    /// <summary>
    /// Represents a powerup that will be applied when the player kills an enemy
    /// using a default bullet.
    /// </summary>
    /// <inheritdoc/>
    public abstract class OnDefaultWeaponKillPowerup : Powerup
    {
        public sealed override bool IsDefaultWeaponPowerup => true;

        public override void OnLevelUp() { }
        public abstract void OnKill(Enemy enemy, DefaultBullet bullet);

        protected sealed override void InitBalance(in PowerupBalanceManager balance)
            => InitBalance(in balance.OnDefaultWeaponKill);

        protected abstract void InitBalance(in PowerupBalanceManager.OnDefaultWeaponKillBalance balance);
    }
}
