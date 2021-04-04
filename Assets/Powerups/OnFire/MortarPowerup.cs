using System;
using System.Linq;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    ///
    /// </summary>
    /// <inheritdoc/>
    public class MortarPowerup : OnFirePowerup
    {
        private float PowerValue => PowerCalculator.Value;
        private SumLevelValueCalculator PowerCalculator { get; set; }

        protected override void InitBalance(in PowerupBalanceManager.OnFireBalance balance)
        {
            float powerBase = balance.Mortar.Power.Base;
            float powerIncrease = balance.Mortar.Power.Increase;
            PowerCalculator = new SumLevelValueCalculator(powerBase, powerIncrease);
        }

        private bool _firingLeft;
        private bool FiringLeft
        {
            get
            {
                _firingLeft = !_firingLeft;
                return _firingLeft;
            }
        }
        private Vector3 FirePosition => FiringLeft ? SpaceUtil.WorldMap.BottomLeft : SpaceUtil.WorldMap.BottomRight;

        public override void OnFire(Vector3 position, PlayerBullet[] bullets)
        {
            int max = Math.Min(bullets.Length, 2);

            for (int i = 0; i < max; i++)
                FireMortar(position);
        }

        public void FireMortar(Vector3 position)
        {
            var firePosition = FirePosition;
            var destination = SpaceUtil.WorldMap.Center;
            destination.x = position.x;
            var bullet = PoolManager.Instance.BulletPool.Get<MortarBullet>(firePosition);

            var velocity = MathUtil.VelocityVector(firePosition, destination, bullet.Speed);
            bullet.Velocity = velocity;
        }
    }
}
