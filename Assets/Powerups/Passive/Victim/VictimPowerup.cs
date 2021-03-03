﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.ObjectPooling;
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
        private const float FireSpeedBase = 0.5f;
        private const float FireSpeedScale = 0.85f;

        protected override LevelValueCalculator InitialValueCalculator
            => new ProductLevelValueCalculator(FireSpeedBase, FireSpeedScale);

        private const float DamageBase = 0;
        private const float DamageIncrease = 5;
        private SumLevelValueCalculator DamageCalculator =
            new SumLevelValueCalculator(DamageBase, DamageIncrease);

        public int Damage => (int)DamageCalculator.Value;

        private LoopingFrameTimer FireTimer { get; set; }

        public VictimPowerup()
        {
            FireTimer = LoopingFrameTimer.Default();
        }

        public override void OnLevelUp()
        {
            if (Level == 1)
                Player.Instance.VictimMarker = PoolManager.Instance.UIElementPool.Get<VictimMarker>();

            DamageCalculator.Level = Level;

            FireTimer.ActivationInterval = ValueCalculator.Value;
            FireTimer.ActivateSelf();
        }

        public override void RunFrame(float deltaTime)
        {
            if (Input.GetMouseButtonDown(0) && SpaceUtil.TryGetEnemyUnderMouse(out Enemy victim))
                victim.IsVictim = true;


            if(FireTimer.UpdateActivates(deltaTime))
                FireAtVictim();
        }

        public void FireAtVictim()
        {
            Enemy victim = GameManager.Instance.VictimEnemy;

            if (victim != null)
            {
                var firePosition = Player.Instance.FirePosition();
                var bullet = PoolManager.Instance.BulletPool.Get<VictimBullet>(firePosition, ValueCalculator.Level);

                var velocity = MathUtil.VelocityVector(firePosition, victim.transform.position, bullet.Speed);
                velocity += victim.Velocity;

                bullet.Velocity = velocity;
                bullet.DamageIncrease = Damage;
            }
        }
    }
}