﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets;
using Assets.Bullets.PlayerBullets;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Represents a powerup that will be applied when the player fires their main cannon.
    /// </summary>
    /// <inheritdoc/>
    public abstract class OnFirePowerup : Powerup
    {
        public override void OnLevelUp() { }
        public abstract void OnFire(Vector2 position, PlayerBullet[] bullets);
    }
}
