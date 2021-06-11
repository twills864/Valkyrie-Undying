using System;
using System.Linq;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Enemies;
using Assets.FireStrategyManagers;
using Assets.GameTasks;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.UI.SpriteBank;
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
        const int MaxBullets = GameConstants.MaxWeaponLevel + 2;
        const int NumLanes = (MaxBullets * 2) - 1;

        const int MiddleLane = (NumLanes / 2);

        protected override Sprite GetPickupSprite(HeavyWeaponSpriteBank bank) => bank.Diamond;

        private FireLaneWithAngle[] FireLanes { get; } = new FireLaneWithAngle[NumLanes];

        private int DiamondLevel;
        private DiamondLevelDelta _DiamondLevelDelta = DiamondLevelDelta.Increasing;

        private enum DiamondLevelDelta
        {
            Increasing = 1,
            Decreasing = -1
        }

        public DeadlyDiamondStrategy(DeadlyDiamondBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
        {
            var template = PoolManager.Instance.BulletPool.Get<DeadlyDiamondBullet>();

            float offsetX = template.OffsetX;
            float maxAngle = template.MaxAngle;
            float speed = template.Speed;

            template.DeactivateSelf();

            float offsetDelta = (offsetX * NumLanes - 1) * 0.0625f;

            const float AngleUp = 90f;
            float angleDelta = maxAngle / MaxBullets;

            for (int i = 0; i < NumLanes; i++)
            {
                int laneDelta = i - MiddleLane;

                float x = laneDelta * offsetDelta;
                Vector3 spawn = new Vector3(x, 0f);

                float angle = AngleUp - (laneDelta * angleDelta);
                float angleAssignment = angle + ((angle - AngleUp) * 0.75f);

                var velocity = MathUtil.Vector2AtDegreeAngle(angle, speed);

                FireLanes[i] = new FireLaneWithAngle(spawn, velocity, angleAssignment);
            }
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

            int currentLane = MiddleLane - (DiamondLevel - 1);

            for (int i = 0; i < DiamondLevel; i++)
            {
                var lane = FireLanes[currentLane];
                var diamond = ret[i];

                lane.ApplyToSprite(diamond, playerFirePos);

                // Skip a lane each time
                currentLane += 2;
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

        #region // Manually calculated fire
        //public override PlayerBullet[] GetBullets(int weaponLevel, Vector3 playerFirePos)
        //{
        //    const int AngleDeltaDenominator = (MaxBullets / 2);

        //    DeadlyDiamondBullet[] ret = PoolManager.Instance.BulletPool.GetMany<DeadlyDiamondBullet>(DiamondLevel);

        //    var first = ret[0];
        //    float offsetX = first.OffsetX;
        //    float maxAngle = first.MaxAngle;
        //    float speed = first.Speed;

        //    const float AngleUp = 90f;
        //    float angleDelta = maxAngle / AngleDeltaDenominator;
        //    float angle = AngleUp + (0.5f * angleDelta * (ret.Length - 1));

        //    Vector3 spawn = playerFirePos;
        //    spawn.x -= (offsetX * (ret.Length - 1) * 0.5f);

        //    for (int i = 0; i < ret.Length; i++)
        //    {
        //        var diamond = ret[i];
        //        diamond.transform.position = spawn;
        //        diamond.RotationDegrees = angle + ((angle - AngleUp) * 0.75f);

        //        var velocity = MathUtil.Vector2AtDegreeAngle(angle, speed);
        //        diamond.Velocity = velocity;

        //        if (!SpaceUtil.PointIsInBounds(spawn))
        //            diamond.RunTask(GameTaskFunc.DeactivateSelf(ret[i]));

        //        spawn.x += offsetX;
        //        angle -= angleDelta;
        //    }

        //    AdjustDiamondLevel(weaponLevel);
        //    return ret;
        //}
        #endregion // Manually calculated fire
    }
}
