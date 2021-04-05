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
    /// Represents a powerup that will be applied when the player hits an enemy.
    /// </summary>
    /// <inheritdoc/>
    public abstract class OnHitPowerup : Powerup
    {
        public override void OnLevelUp() { }
        public abstract void OnHit(Enemy enemy, PlayerBullet bullet, Vector3 hitPosition);

        protected sealed override void InitBalance(in PowerupBalanceManager balance)
            => InitBalance(in balance.OnHit);

        protected abstract void InitBalance(in PowerupBalanceManager.OnHitBalance balance);
    }
}
