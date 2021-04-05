using System;
using System.Linq;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.FireStrategyManagers;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <summary>
    ///
    /// </summary>
    /// <inheritdoc/>
    public class DeadlyDiamondStrategy : PlayerFireStrategy<DeadlyDiamondBullet>
    {
        const int MaxBullets = GameConstants.MaxWeaponLevel + 1;
        const int NumLanes = (MaxBullets * 2) - 1;

        const int AngleDeltaDenominator = (MaxBullets / 2);

        private int DiamondLevel;
        private DiamondLevelDelta _DiamondLevelDelta;

        private enum DiamondLevelDelta
        {
            Increasing = 1,
            Decreasing = -1
        }

        public DeadlyDiamondStrategy(DeadlyDiamondBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
        {
        }

        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios)
            => ratios.DeadlyDiamond;

        public override void OnActivate()
        {
            DiamondLevel = 1;
            _DiamondLevelDelta = DiamondLevelDelta.Increasing;
        }

        public override PlayerBullet[] GetBullets(int weaponLevel, Vector3 playerFirePos)
        {
            DeadlyDiamondBullet[] ret = PoolManager.Instance.BulletPool.GetMany<DeadlyDiamondBullet>(DiamondLevel);

            var first = ret[0];
            float offsetX = first.OffsetX;
            float maxAngle = first.MaxAngle;
            float speed = first.Speed;

            const float AngleUp = 90f;
            float angleDelta = maxAngle / AngleDeltaDenominator;
            float angle = AngleUp + (0.5f * angleDelta * (ret.Length - 1));

            Vector3 spawn = playerFirePos;
            spawn.x -= (offsetX * (ret.Length - 1) * 0.5f);

            for (int i = 0; i < ret.Length; i++)
            {
                var diamond = ret[i];
                diamond.transform.position = spawn;
                diamond.RotationDegrees = angle + ((angle - AngleUp) * 0.75f);

                var velocity = MathUtil.Vector2AtDegreeAngle(angle, speed);
                diamond.Velocity = velocity;

                if (!SpaceUtil.WorldMap.ContainsPoint(spawn))
                    diamond.RunTask(GameTaskFunc.DeactivateSelf(ret[i]));

                spawn.x += offsetX;
                angle -= angleDelta;
            }

            AdjustDiamondLevel(weaponLevel);
            return ret;
        }

        private void AdjustDiamondLevel(int weaponLevel)
        {
            weaponLevel++;

            if (DiamondLevel > weaponLevel)
                _DiamondLevelDelta = DiamondLevelDelta.Decreasing;

            else if (DiamondLevel <= 1)
                _DiamondLevelDelta = DiamondLevelDelta.Increasing;

            DiamondLevel += (int)_DiamondLevelDelta;

        }
    }
}
