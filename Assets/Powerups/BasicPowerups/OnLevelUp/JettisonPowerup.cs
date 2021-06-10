using System;
using System.Linq;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.UI.SpriteBank;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    ///
    /// </summary>
    /// <inheritdoc/>
    public class JettisonPowerup : OnLevelUpPowerup
    {
        public override int MaxLevel => 1;
        protected override Sprite GetPowerupSprite(PowerupSpriteBank bank) => bank.Jettison;

        private float ScaleTime { get; set; }

        private float ScaleX => ScaleXCalculator.Value;
        private ProductLevelValueCalculator ScaleXCalculator { get; set; }

        protected override void InitBalance(in PowerupBalanceManager.OnLevelUpBalance balance)
        {
            ScaleTime = balance.Jettison.ScaleTime;

            float scaleXIncrease = balance.Jettison.ScaleXPerLevel;
            float scaleXBase = Player.Instance.InitialScale.x * scaleXIncrease;
            ScaleXCalculator = new ProductLevelValueCalculator(scaleXBase, scaleXIncrease);
        }

        public override void OnLevelUp()
        {
            Vector3 scale = Player.Instance.InitialScale;
            scale.x = ScaleX;

            var scaleTo = new ScaleTo(Player.Instance, scale, ScaleTime);
            Player.Instance.RunTask(scaleTo);
        }
    }
}
