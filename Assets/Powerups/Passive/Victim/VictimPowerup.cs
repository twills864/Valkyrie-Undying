using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.UI;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// The player can click an enemy that will be the victim of homing bullets.
    /// </summary>
    /// <inheritdoc/>
    public class VictimPowerup : PassivePowerup
    {
        protected override void InitBalance(in PowerupBalanceManager.PassiveBalance balance)
        {
            float fireSpeedBase = balance.Victim.FireSpeed.Base;
            float fireSpeedScale = balance.Victim.FireSpeed.ScalePerLevel;
            FireTimeCalculator = new ProductLevelValueCalculator(fireSpeedBase, fireSpeedScale);

            float damageBase = balance.Victim.Damage.Base;
            float damageIncrease = balance.Victim.Damage.Increase;
            DamageCalculator = new SumLevelValueCalculator(damageBase, damageIncrease);
        }

        public float FireTime => FireTimeCalculator.Value;
        private ProductLevelValueCalculator FireTimeCalculator { get; set; }

        public int Damage => (int)DamageCalculator.Value;
        private SumLevelValueCalculator DamageCalculator { get; set; }

        private LoopingFrameTimer FireTimer { get; } = LoopingFrameTimer.Default();

        public override void OnLevelUp()
        {
            if (Level == 1)
            {
                Player.Instance.VictimMarker = PoolManager.Instance.UIElementPool.Get<VictimMarker>();
                Director.StartCheckingForVictim();
            }

            FireTimer.ActivationInterval = FireTimeCalculator.Value;
            FireTimer.ActivateSelf();
        }

        public override void RunFrame(float deltaTime, float realDeltaTime)
        {
            if (Input.GetMouseButtonDown(0) && SpaceUtil.TryGetEnemyUnderMouse(out Enemy victim))
            {
                victim.IsVictim = true;
                GameManager.Instance.VictimWasAutomatic = false;
            }

            if(FireTimer.UpdateActivates(deltaTime))
                FireAtVictim();
        }

        public void FireAtVictim()
        {
            Enemy victim = GameManager.Instance.VictimEnemy;

            if (victim != null)
            {
                var firePosition = Player.Instance.FirePosition;
                var bullet = PoolManager.Instance.BulletPool.Get<VictimBullet>(firePosition, FireTimeCalculator.Level);

                var velocity = MathUtil.VelocityVector(firePosition, victim.transform.position, bullet.Speed);
                velocity += victim.Velocity;

                bullet.Velocity = velocity;
                bullet.VictimDamage = Damage;
            }
        }
    }
}
