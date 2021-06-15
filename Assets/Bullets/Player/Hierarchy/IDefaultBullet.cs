using System;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.Powerups;
using Assets.ScreenEdgeColliders;
using Assets.Util;
using UnityEngine;

namespace Assets.Bullets.PlayerBullets
{
    /// <inheritdoc/>
    public interface IDefaultBullet
    {
        int NumberPenetrated { get; set; }
        int VenomousRoundsDamage { get; set; }
        int SnakeBiteDamage { get; set; }
        int ParasiteDamage{ get; set; }
    }
}