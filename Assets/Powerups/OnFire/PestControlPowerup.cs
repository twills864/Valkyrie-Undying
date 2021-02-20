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
        private const float NumPelletsBase = 0;
        private const float NumPelletsIncrease = 1;

        protected override LevelValueCalculator InitialValueCalculator
            => new SumLevelValueCalculator(NumPelletsBase, NumPelletsIncrease);

        private float NumPellets => InitialValueCalculator.Level;

        public override void OnFire(Vector2 position, PlayerBullet[] bullets)
        {
            int pestControlCounter = 0;

            foreach(var bullet in bullets)
            {
                int damage = bullet.Damage;

                if (RandomUtil.BoolPercent(damage))
                    pestControlCounter++;
            }

            if(pestControlCounter > 0)
                GameManager.Instance.FirePestControl(position, pestControlCounter);
        }


    }
}
