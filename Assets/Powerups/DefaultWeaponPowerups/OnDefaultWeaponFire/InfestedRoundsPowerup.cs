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
    /// Adds parasite damage to fired default bullets.
    /// </summary>
    /// <inheritdoc/>
    public class InfestedRoundsPowerup : OnDefaultWeaponFirePowerup
    {
        public override int MaxLevel => 1;
        protected override Sprite GetPowerupSprite(PowerupSpriteBank bank) => bank.InfestedRounds;

        private int NumParasites => (int) NumParasitesCalculator.Value;
        private SumLevelValueCalculator NumParasitesCalculator { get; set; }

        protected override void InitBalance(in PowerupBalanceManager.OnDefaultWeaponFireBalance balance)
        {
            float numParasitesBase = balance.InfestedRounds.NumParasites.Base;
            float numParasitesIncrease = balance.InfestedRounds.NumParasites.Increase;
            NumParasitesCalculator = new SumLevelValueCalculator(numParasitesBase, numParasitesIncrease);
        }

        public override void OnFire(Vector3 position, DefaultBullet[] bullets)
        {
            foreach (var bullet in bullets)
                bullet.ParasiteDamage = NumParasites;
        }
    }
}
