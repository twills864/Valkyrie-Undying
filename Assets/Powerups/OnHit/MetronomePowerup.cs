using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Does increasing amounts of damage to an enemy that's hit multiple times in a row.
    /// </summary>
    /// <inheritdoc/>
    public class MetronomePowerup : OnHitPowerup
    {
        protected override void InitBalance(in PowerupBalanceManager.OnHitBalance balance)
        {
            //float exponentBase = balance.Metronome.Damage.ExponentBase;
            //float maxRatio = balance.Metronome.Damage.MaxRatio;
            //MetronomeScaleCalculator = new AsymptoteScaleLevelValueCalculator(exponentBase, maxRatio);

            float baseValue = balance.Metronome.Damage.BaseValue;
            float exponentBase = balance.Metronome.Damage.ExponentBase;
            float scale = balance.Metronome.Damage.Scale;
            MetronomeScaleCalculator = new AsymptoteRatioLevelValueCalculator(baseValue, exponentBase, scale);
        }

        private float MetronomeScale => MetronomeScaleCalculator.Value;
        private AsymptoteRatioLevelValueCalculator MetronomeScaleCalculator { get; set; }

        private float Increase;

        public override void OnHit(Enemy enemy, PlayerBullet bullet)
        {
            if(enemy.HasMetronome)
            {
                ApplyIncrease();
            }
            else
            {
                ResetIncrease();
                enemy.HasMetronome = true;
            }
            enemy.MetronomeLabel.Text = Increase;

            DebugUI.SetDebugLabel("Increase Powerup", () => Increase);
            DebugUI.SetDebugLabel("MetronomeScale", () => MetronomeScale);
        }

        private void ApplyIncrease()
        {
            Increase += MetronomeScale;
        }

        private void ResetIncrease()
        {
            Increase = MetronomeScale;
        }
    }
}
