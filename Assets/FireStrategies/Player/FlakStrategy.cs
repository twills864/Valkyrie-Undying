//#define FIREALL

using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Util;
using Assets.ObjectPooling;
using UnityEngine;
using Assets.FireStrategyManagers;
using Assets.UI.SpriteBank;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <inheritdoc/>
    public class FlakStrategy : PlayerFireStrategy<FlakBullet>
    {
        private const int NumGuaranteedPellets = 1;
        private const int NumAdditionalPelletLanes = 14;
        private const int TotalPelletLanes = NumGuaranteedPellets + NumAdditionalPelletLanes;

        // Bullets fire in loose pyramid shape
        private const int RowsInPyramid = 5;

        protected override Sprite GetPickupSprite(HeavyWeaponSpriteBank bank) => bank.Flak;

        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios) => ratios.Flak;

        //public override LoopingFrameTimer FireTimer { get; protected set; }
        //    = new LoopingFrameTimer(0.5f);

        // Maps loop indexes to their matching assigned lane index
        private FireLane[] FireLanes { get; } = new FireLane[TotalPelletLanes];

        private float BulletVelocityY;
        private Vector2 BulletSize;
        private float BulletOffsetX;
        private float BulletOffsetY;
        private float BulletSpreadX;
        private float BulletSpreadY;

        public FlakStrategy(FlakBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
        {
            BulletVelocityY = bullet.BulletVelocityY;
            BulletSize = bullet.GetComponent<Renderer>().bounds.size;
            BulletOffsetX = bullet.BulletOffsetX;
            BulletOffsetY = bullet.BulletOffsetY;
            BulletSpreadX = bullet.BulletSpreadX;
            BulletSpreadY = bullet.BulletSpreadY;

            FillAssignedLanesPositions();
            FillAssignedLanesVelocities();
        }

        public override PlayerBullet[] GetBullets(int weaponLevel, Vector3 playerFirePos)
        {
#if !FIREALL
            int numAdditional = 2 + PlusOneIfMaxLevel(weaponLevel);
            int numToGet = NumGuaranteedPellets + numAdditional;
#else
            int numAdditional = TotalPelletLanes - NumGuaranteedPellets;
            int numToGet = TotalPelletLanes;
#endif

            bool[] shouldFireLanes = RandomUtil.ShuffledBools(NumAdditionalPelletLanes, numAdditional);
            FlakBullet[] ret = PoolManager.Instance.BulletPool.GetMany<FlakBullet>(numToGet);

            FireGuaranteedLanes(ret, playerFirePos);

            int nextFireIndex = NumGuaranteedPellets;
            // All other lanes fire randomly based on weaponLevel.
            int i;
            for (i = 0; i < NumAdditionalPelletLanes; i++)
            {
                bool shouldFireLane = shouldFireLanes[i];

#if !FIREALL
                if (shouldFireLane)
#endif
                    FireLane(ret[nextFireIndex++], i + NumGuaranteedPellets, playerFirePos);
            }

            return ret;
        }

        private void FireLane(FlakBullet bullet, int laneIndex, Vector3 playerFirePos)
        {
            var lane = FireLanes[laneIndex];
            lane.ApplyToSprite(bullet, playerFirePos);
        }

        private void FireGuaranteedLanes(FlakBullet[] ret, Vector3 playerFirePos)
        {
            for(int i = 0; i < NumGuaranteedPellets; i++)
                FireLane(ret[i], i, playerFirePos);
        }

        /// <summary>
        /// Initializes the starting positions of each potential fire lane.
        /// The bullets are fired in a loose pyramid pattern.
        /// </summary>
        private void FillAssignedLanesPositions()
        {
            // Add a little bit of Y variance to make the pyramid shape less obvious
            float yOffsetVariance = BulletOffsetY * 0.25f;

            int laneCounter = 0;
            for(int y = 0; y < RowsInPyramid; y++)
            {
                float yOffset = y * BulletOffsetY;
                float xStart = y * -0.5f;
                for (int x = 0; x <= y; x++)
                {
                    float xOffset = (xStart + x) * BulletOffsetX;
                    float newYOffset = yOffset + yOffsetVariance;
                    FireLanes[laneCounter++].PositionOffset = new Vector3(xOffset, newYOffset);

                    yOffsetVariance = -yOffsetVariance;
                }
            }
        }

        /// <summary>
        /// Initializes the velocities of each potential fire lane.
        /// The Y velocities are correlated higher with higher Y starting positions.
        /// </summary>
        private void FillAssignedLanesVelocities()
        {
            int laneCounter = 0;
            for (int y = 0; y < RowsInPyramid; y++)
            {
                float yVelocity = (y * BulletSpreadY) + BulletVelocityY;
                float xStart = y * -0.5f;
                for (int x = 0; x <= y; x++)
                {
                    float xVelocity = (xStart + x) * BulletSpreadX;
                    FireLanes[laneCounter++].Velocity = new Vector2(xVelocity, yVelocity);
                }
            }
        }
    }
}
