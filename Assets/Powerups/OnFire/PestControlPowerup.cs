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
    /// Randomly spawns damaging shrapnel behind a hit enemy.
    /// </summary>
    /// <inheritdoc/>
    public class PestControlPowerup : OnFirePowerup
    {
        protected override void InitBalance(in PowerupBalanceManager.OnFireBalance balance)
        {
            float exponentRatio = balance.PestControl.ExponentRatio;
            float maxValue = balance.PestControl.MaxValue;

            ChanceModifierCalculator = new AsymptoteScaleLevelValueCalculator(exponentRatio, maxValue);
        }

        private float ChanceModifier => ChanceModifierCalculator.Value;
        private AsymptoteScaleLevelValueCalculator ChanceModifierCalculator { get; set; }


        private EnemyBulletPoolList EnemyBulletPoolList;
        private ObjectPool<PlayerBullet> PestControlPool;

        public void Init()
        {
            EnemyBulletPoolList = PoolManager.Instance.EnemyBulletPool;
            PestControlPool = PoolManager.Instance.BulletPool.GetPool<PestControlBullet>();
        }

        public override void OnFire(Vector3 position, PlayerBullet[] bullets)
        {
            int pestControlCounter = bullets
                .Select(bullet => bullet.PestControlChance * ChanceModifier)
                .Where(chance => RandomUtil.Bool(chance))
                .Count();

            if (pestControlCounter > 0)
                FirePestControl(position, pestControlCounter);
        }

        private void FirePestControl(Vector3 position, int numberToGet)
        {
            var targets = EnemyBulletPoolList.GetPestControlTargets(numberToGet);

            numberToGet = targets.Length;
            var pestControls = PestControlPool.GetMany<PestControlBullet>(numberToGet);

            for (int i = 0; i < pestControls.Length; i++)
            {
                var pestControl = pestControls[i];
                var target = targets[i];

                pestControl.SetTarget(position, target);
            }
        }
    }
}
