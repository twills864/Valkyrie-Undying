using System.Linq;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Randomly fires a Pest Control bullet with the main cannon.
    /// Pest Control bullets lock onto a random enemy bullet, and
    /// reflect it on contact.
    /// </summary>
    /// <inheritdoc/>
    public class PestControlPowerup : OnFirePowerup
    {
        private float BulletDamageBalanceBase { get; set; }

        protected override void InitBalance(in PowerupBalanceManager.OnFireBalance balance)
        {
            BulletDamageBalanceBase = 1f / balance.PestControl.BulletDamageBalanceBase;

            float baseChance = balance.PestControl.BaseChance;
            float chanceIncrease = balance.PestControl.ChanceIncrease;

            ChanceModifierCalculator = new SumLevelValueCalculator(baseChance, chanceIncrease);
        }

        private float ChanceModifier => ChanceModifierCalculator.Value;
        private SumLevelValueCalculator ChanceModifierCalculator { get; set; }


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
                .Where(x => BulletFiresPestControl(x))
                .Count();

            if (pestControlCounter > 0)
                FirePestControl(position, pestControlCounter);
        }

        private bool BulletFiresPestControl(PlayerBullet bullet)
        {
            float damageBalance = bullet.Damage * BulletDamageBalanceBase;
            float chance =  damageBalance * ChanceModifier;

            bool fires = RandomUtil.Bool(chance);
            return fires;
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

            // Question mark notation because there may be no Pest Controls fired
            // if there aren't any enemy bullets.
            pestControls[0]?.PlayFireSound();
        }
    }
}
