using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.Powerups.Balance;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Represents a powerup that will be applied when the player hits an enemy
    /// with a default bullet.
    /// </summary>
    /// <inheritdoc/>
    public abstract class OnDefaultWeaponHitPowerup : DefaultWeaponPowerup
    {
        public override void OnLevelUp() { }
        public abstract void OnHit(Enemy enemy, DefaultBullet bullet, Vector3 hitPosition);

        protected sealed override void InitBalance(in PowerupBalanceManager balance)
            => InitBalance(in balance.OnDefaultWeaponHit);

        protected abstract void InitBalance(in PowerupBalanceManager.OnDefaultWeaponHitBalance balance);
    }
}
