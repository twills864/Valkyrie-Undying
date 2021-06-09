using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.Util;

namespace Assets.Powerups
{
    /// <summary>
    /// Periodically shoots a fireball that ignites enemies on hit.
    /// </summary>
    /// <inheritdoc/>
    public class InfernoPowerup : PassivePowerup
    {
        public override int MaxLevel => 2;

        protected override void InitBalance(in PowerupBalanceManager.PassiveBalance balance)
        {
            float damageIncreaseBase = balance.Inferno.Damage.Base;
            float damageIncreasePerLevel = balance.Inferno.Damage.Increase;
            DamageCalculator = new SumLevelValueCalculator(damageIncreaseBase, damageIncreasePerLevel);

            float maxDamageBase = balance.Inferno.MaxDamage.Base;
            float maxDamageIncrease = balance.Inferno.MaxDamage.Increase;
            MaxDamageCalculator = new SumLevelValueCalculator(maxDamageBase, maxDamageIncrease);

            float fireSpeedBase = balance.Inferno.FireSpeed.Base;
            float fireSpeedIncrease = balance.Inferno.FireSpeed.ScalePerLevel;
            FireSpeedCalculator = new ProductLevelValueCalculator(fireSpeedBase, fireSpeedIncrease);
        }

        public float FireSpeed => FireSpeedCalculator.Value;
        private ProductLevelValueCalculator FireSpeedCalculator { get; set; }

        public int DamageIncrease => (int) DamageCalculator.Value;
        private SumLevelValueCalculator DamageCalculator { get; set; }

        public int MaxDamage => (int)MaxDamageCalculator.Value;
        private SumLevelValueCalculator MaxDamageCalculator { get; set; }

        private LoopingFrameTimer FireTimer { get; } = LoopingFrameTimer.Default();
        private ObjectPool<PlayerBullet> InfernoPool { get; set; }

        public InfernoPowerup()
        {
            InfernoPool = PoolManager.Instance.BulletPool.GetPool<InfernoBullet>();
            var bullet = (InfernoBullet)InfernoPool.Get();
            bullet.DeactivateSelf();
        }

        public override void OnLevelUp()
        {
            FireTimer.ActivationInterval = FireSpeed;
            FireTimer.ActivateSelf();
        }

        public override void RunFrame(float deltaTime, float realDeltaTime)
        {
            if(FireTimer.UpdateActivates(deltaTime))
            {
                var firePos = Player.Instance.FirePosition;
                var bullet = (InfernoBullet)InfernoPool.Get();

                bullet.OnSpawn(this, firePos);
            }
        }
    }
}
