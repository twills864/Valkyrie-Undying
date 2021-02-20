using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Randomly spawns damaging shrapnel behind a hit enemy.
    /// </summary>
    /// <inheritdoc/>
    public class PestControlPowerup : OnFirePowerup
    {
        private const float ExponentRatio = 0.8f;
        private const float MaxValue = 2f;

        protected override LevelValueCalculator InitialValueCalculator
            => new AsymptoteRatioLevelValueCalculator(ExponentRatio, MaxValue);

        private float ChanceModifier => ValueCalculator.Value;

        public override void OnFire(Vector2 position, PlayerBullet[] bullets)
        {
            int pestControlCounter = bullets
                .Select(bullet => bullet.PestControlChance * ChanceModifier)
                .Where(chance => RandomUtil.Bool(chance))
                .Count();

            if(pestControlCounter > 0)
                GameManager.Instance.FirePestControl(position, pestControlCounter);
        }


    }
}
