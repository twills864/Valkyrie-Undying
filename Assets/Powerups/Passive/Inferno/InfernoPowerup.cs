using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.ObjectPooling;
using Assets.Util;

namespace Assets.Powerups
{
    /// <summary>
    /// Periodically shoots a fireball that ignites enemies on hit.
    /// </summary>
    /// <inheritdoc/>
    public class InfernoPowerup : PassivePowerup
    {
        public static int CurrentBaseDamage { get; private set; }
        public static int CurrentDamageIncreasePerTick { get; private set; }

        private const float FireSpeedBase = 3.0f;
        private const float FireSpeedIncrease = 0.8f;

        protected override LevelValueCalculator InitialValueCalculator
            => new ProductLevelValueCalculator(FireSpeedBase, FireSpeedIncrease);

        public float FireSpeed => ValueCalculator.Value;

        private const float DamageIncreaseBase = 1;
        public const float DamageIncreasePerLevel = 1;
        private SumLevelValueCalculator DamageCalculator =
            new SumLevelValueCalculator(DamageIncreaseBase, DamageIncreasePerLevel);

        private int DamageIncrease => (int) DamageCalculator.Value;

        private LoopingFrameTimer FireTimer { get; set; }
        private ObjectPool<PlayerBullet> InfernoPool { get; set; }

        public InfernoPowerup()
        {
            FireTimer = LoopingFrameTimer.Default();
            InfernoPool = PoolManager.Instance.BulletPool.GetPool<InfernoBullet>();
            var bullet = (InfernoBullet)InfernoPool.Get();
            CurrentBaseDamage = bullet.Damage;
            bullet.DeactivateSelf();
        }

        public override void OnLevelUp()
        {
            DamageCalculator.Level = Level;
            CurrentDamageIncreasePerTick = DamageIncrease;

            FireTimer.ActivationInterval = FireSpeed;
            FireTimer.ActivateSelf();
        }

        public override void RunFrame(float deltaTime)
        {
            if(FireTimer.UpdateActivates(deltaTime))
            {
                var firePos = Player.Instance.FirePosition();
                var bullet = (InfernoBullet)InfernoPool.Get();
                bullet.transform.position = firePos;
                bullet.BulletLevel = Level;
                bullet.DamageIncreasePerTick = DamageIncrease;
            }
        }
    }
}
