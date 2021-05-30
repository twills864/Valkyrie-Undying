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

        public static int CurrentBaseDamage { get; private set; }
        public static int CurrentDamageIncreasePerTick { get; private set; }
        public static int CurrentMaxDamage { get; private set; }

        public static float DamageIncreasePerLevel;

        protected override void InitBalance(in PowerupBalanceManager.PassiveBalance balance)
        {
            float damageIncreaseBase = balance.Inferno.Damage.Base;
            DamageIncreasePerLevel = balance.Inferno.Damage.Increase;
            DamageCalculator = new SumLevelValueCalculator(damageIncreaseBase, DamageIncreasePerLevel);

            float maxDamageBase = balance.Inferno.MaxDamage.Base;
            float maxDamageIncrease = balance.Inferno.MaxDamage.Increase;
            MaxDamageCalculator = new SumLevelValueCalculator(maxDamageBase, maxDamageIncrease);

            float fireSpeedBase = balance.Inferno.FireSpeed.Base;
            float fireSpeedIncrease = balance.Inferno.FireSpeed.ScalePerLevel;
            FireSpeedCalculator = new ProductLevelValueCalculator(fireSpeedBase, fireSpeedIncrease);
        }

        public float FireSpeed => FireSpeedCalculator.Value;
        private ProductLevelValueCalculator FireSpeedCalculator { get; set; }

        private int DamageIncrease => (int) DamageCalculator.Value;
        private SumLevelValueCalculator DamageCalculator { get; set; }

        private int MaxDamage => (int)MaxDamageCalculator.Value;
        private SumLevelValueCalculator MaxDamageCalculator { get; set; }

        private LoopingFrameTimer FireTimer { get; } = LoopingFrameTimer.Default();
        private ObjectPool<PlayerBullet> InfernoPool { get; set; }

        public InfernoPowerup()
        {
            InfernoPool = PoolManager.Instance.BulletPool.GetPool<InfernoBullet>();
            var bullet = (InfernoBullet)InfernoPool.Get();
            CurrentBaseDamage = bullet.Damage;
            bullet.DeactivateSelf();
        }

        public override void OnLevelUp()
        {
            CurrentDamageIncreasePerTick = DamageIncrease;
            CurrentMaxDamage = MaxDamage;

            FireTimer.ActivationInterval = FireSpeed;
            FireTimer.ActivateSelf();
        }

        public override void RunFrame(float deltaTime, float realDeltaTime)
        {
            if(FireTimer.UpdateActivates(deltaTime))
            {
                var firePos = Player.Instance.FirePosition;
                var bullet = (InfernoBullet)InfernoPool.Get();
                bullet.transform.position = firePos;
                bullet.BulletLevel = Level;
                bullet.DamageIncreasePerTick = DamageIncrease;
                bullet.MaxDamage = MaxDamage;

                bullet.PlayFireSound();
            }
        }
    }
}
