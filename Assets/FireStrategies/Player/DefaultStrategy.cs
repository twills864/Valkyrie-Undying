﻿using Assets.Bullets.PlayerBullets;
using Assets.FireStrategyManagers;
using Assets.ObjectPooling;
using Assets.Powerups;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <inheritdoc/>
    public class DefaultStrategy : PlayerFireStrategy<DefaultBullet>
    {
        #region Property Fields
        private float _CurrentFireSpeedModifier = 1f;
        #endregion

        private static DefaultStrategy Instance { get; set; }

        private int NumBulletsToGet { get; set; }

        private float DefaultFireSpeed { get; set; }
        private float CurrentFireSpeedModifier
        {
            get => _CurrentFireSpeedModifier;
            set
            {
                _CurrentFireSpeedModifier = value;
                FireTimer.ActivationInterval = DefaultFireSpeed * CurrentFireSpeedModifier;
                FireTimer.ActivateSelf();
            }
        }

        public DefaultStrategy(DefaultBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
        {
            Instance = this;

            NumBulletsToGet = 1;

            DefaultFireSpeed = manager.BaseFireSpeed;
        }

        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios)
        {
            // Default ratio is 1, since the fire speed ratio is
            // fire speed compared to the default strategy.
            const float DefaultRatio = 1.0f;
            return DefaultRatio;
        }

        public override PlayerBullet[] GetBullets(int weaponLevel, Vector3 playerFirePos)
        {
            DefaultBullet[] bullets = PoolManager.Instance.BulletPool
                .GetMany<DefaultBullet>(NumBulletsToGet, playerFirePos, weaponLevel);

            GameManager.Instance.OnDefaultWeaponFire(bullets);

            return bullets;
        }

        public static void ApplySnakeBite(SnakeBitePowerup powerup)
        {
            const int NumSnakeBiteBullets = 2;
            Instance.NumBulletsToGet = NumSnakeBiteBullets;
            Instance.CurrentFireSpeedModifier *= powerup.FireSpeedRatio;
        }
    }
}