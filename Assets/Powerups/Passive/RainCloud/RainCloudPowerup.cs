﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Powerups.Balance;
using Assets.Util;

namespace Assets.Powerups
{
    /// <summary>
    /// Spawns a passive rain cloud behind the player that rains bullets on enemies.
    /// </summary>
    /// <inheritdoc/>
    public class RainCloudPowerup : PassivePowerup
    {
        protected override void InitBalance(in PowerupBalanceManager.PassiveBalance balance)
        {
            float fireSpeedBase = balance.Monsoon.FireSpeed.Base;
            float fireSpeedIncrease = balance.Monsoon.FireSpeed.Increase;
            FireSpeedCalculator = new ProductLevelValueCalculator(fireSpeedBase, fireSpeedIncrease);

            float damageBase = balance.Monsoon.Damage.Base;
            float damageIncrease = balance.Monsoon.Damage.IncreasePerLevel;
            DamageCalculator = new SumLevelValueCalculator(damageBase, damageIncrease);
        }

        //private const float FireSpeedBase = 0.5f;
        //private const float FireSpeedIncrease = 0.9f;

        //private const float DamageBase = 10;
        //private const float DamageIncrease = 1;

        public float FireSpeed => FireSpeedCalculator.Value;
        private ProductLevelValueCalculator FireSpeedCalculator { get; set; }

        public int Damage => (int) DamageCalculator.Value;
        private SumLevelValueCalculator DamageCalculator { get; set; }

        public override void OnLevelUp()
        {
            if(Level == 1)
                RainCloudSpawner.Instance.Activate();

            RainCloud.Instance.LevelUp(Damage, FireSpeed);
        }

        public override void RunFrame(float deltaTime)
        {
            if (RainCloudSpawner.Instance.isActiveAndEnabled)
                RainCloudSpawner.Instance.RunFrame(deltaTime);

            if (RainCloud.Instance.isActiveAndEnabled)
                RainCloud.Instance.RunFrame(deltaTime);
        }
    }
}
