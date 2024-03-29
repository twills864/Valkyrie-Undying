﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.UI;
using Assets.UI.SpriteBank;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Automatically targets a single enemy at a time to be the victim of homing bullets.
    /// The player may also click an enemy to specifically set the target victim.
    /// </summary>
    /// <inheritdoc/>
    public class VictimPowerup : PassivePowerup
    {
        protected override Sprite GetPowerupSprite(PowerupSpriteBank bank) => bank.Victim;

        public float FireTime => FireTimeCalculator.Value;
        private ProductLevelValueCalculator FireTimeCalculator { get; set; }

        public int Damage => (int)DamageCalculator.Value;
        private SumLevelValueCalculator DamageCalculator { get; set; }

        protected override void InitBalance(in PowerupBalanceManager.PassiveBalance balance)
        {
            float fireSpeedBase = balance.Victim.FireSpeed.Base;
            float fireSpeedScale = balance.Victim.FireSpeed.ScalePerLevel;
            FireTimeCalculator = new ProductLevelValueCalculator(fireSpeedBase, fireSpeedScale);

            float damageBase = balance.Victim.Damage.Base;
            float damageIncrease = balance.Victim.Damage.Increase;
            DamageCalculator = new SumLevelValueCalculator(damageBase, damageIncrease);

            TouchRadius = balance.Victim.TouchRadius;
        }

        private LoopingFrameTimer FireTimer { get; } = LoopingFrameTimer.Default();

        private float TouchRadius { get; set; }

        public override void OnLevelUp()
        {
            if (Level == 1)
            {
                if(Director.TryGetRandomEnemy(out Enemy enemy))
                {
                    enemy.IsVictim = true;
                }
                else
                {
                    Player.Instance.VictimMarker = PoolManager.Instance.UIElementPool.Get<VictimMarker>();
                }
                Director.StartCheckingForVictim();
            }

            FireTimer.ActivationInterval = FireTimeCalculator.Value;
            FireTimer.ActivateSelf();
        }

        public override void RunFrame(float deltaTime, float realDeltaTime)
        {
            if (Input.GetMouseButtonDown(0) && TryGetVictimUnderMouse(out Enemy victim))
            {
                victim.IsVictim = true;
                GameManager.Instance.VictimWasAutomatic = false;
            }

            if(FireTimer.UpdateActivates(deltaTime))
                FireAtVictim();
        }

        private bool TryGetVictimUnderMouse(out Enemy victim)
        {
            var mousePos = SpaceUtil.WorldPositionUnderMouse();

            //DebugUtil.RedX(mousePos + new Vector3(TouchRadius, 0));
            //DebugUtil.RedX(mousePos + new Vector3(-TouchRadius, 0));
            //DebugUtil.RedX(mousePos + new Vector3(0, TouchRadius));
            //DebugUtil.RedX(mousePos + new Vector3(0, -TouchRadius));

            #region // Detect Multiple Collisions

//            var collisions = Physics2D.OverlapCircleAll(mousePos, TouchRadius, LayerUtil.LayerEnemies);

//            if (collisions.Any())
//            {
//#if UNITY_EDITOR
//                var distances = new List<Tuple<float, Enemy>>();
//                for(int i = 0; i < collisions.Length; i++)
//                {
//                    Enemy enemy = collisions[i].GetComponent<Enemy>();
//                    float distance = Vector2.Distance(enemy.transform.position, mousePos);
//                    distances.Add(new Tuple<float, Enemy>(distance, enemy));
//                }
//#endif

//                victim = collisions[0].GetComponent<Enemy>();
//                return true;
//            }

            #endregion // Detect Multiple Collisions

            var collision = Physics2D.OverlapCircle(mousePos, TouchRadius, LayerUtil.LayerEnemies);

            if (collision != null)
            {
                victim = collision.GetComponent<Enemy>();
                return true;
            }
            else
            {
                victim = null;
                return false;
            }
        }

        private void FireAtVictim()
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

                bullet.OnSpawn();

                bullet.PlayFireSound();
            }
        }
    }
}
