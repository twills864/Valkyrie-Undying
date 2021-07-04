using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.UI.SpriteBank;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Opens a void on killing an enemy. This void Void Pauses any enemies
    /// it collides with, and also spawns smallers bullets from it center
    /// while it's open.
    /// </summary>
    /// <inheritdoc/>
    public class VoidPowerup : OnKillPowerup
    {
        public override int MaxLevel => 2;
        protected override Sprite GetPowerupSprite(PowerupSpriteBank bank) => bank.Void;

        private float Duration => DurationCalculator.Value;
        private SumLevelValueCalculator DurationCalculator { get; set; }

        private float SizeScale => SizeScaleLevel.Value;
        private SumLevelValueCalculator SizeScaleLevel { get; set; }

        protected override void InitBalance(in PowerupBalanceManager.OnKillBalance balance)
        {
            float durationBase = balance.Void.Duration.Base;
            float durationIncrease = balance.Void.Duration.Increase;
            DurationCalculator = new SumLevelValueCalculator(durationBase, durationIncrease);

            float sizeScaleBase = balance.Void.SizeScale.Base;
            float sizeScaleIncrease = balance.Void.SizeScale.Increase;
            SizeScaleLevel = new SumLevelValueCalculator(sizeScaleBase, sizeScaleIncrease);
        }

        public override void OnLevelUp()
        {
            SoundManager.PlaySound(VoidBullet.VoidBulletFireSound);


            VoidBullet.StartVoid(SpaceUtil.WorldMap.Center, Level, SizeScale, Duration, true);

            //foreach (var enemy in Director.GetAllActiveEnemies())
            //    VoidEnemy(enemy, false);
        }

        public override void OnKill(Enemy enemy, PlayerBullet bullet)
        {
            VoidEnemy(enemy);
        }

        private void VoidEnemy(Enemy enemy, bool playAudio = true)
        {
            if (enemy.CanVoidPause)
            {
                var position = enemy.ColliderMap.Center;
                VoidBullet.StartVoid(position, Level, SizeScale, Duration, playAudio);
            }
        }
    }
}
