//#define FIREALL

using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Util;
using Assets.Util.ObjectPooling;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <inheritdoc/>
    public class FlakStrategy : PlayerFireStrategy<FlakBullet>
    {
        private const int NumGuaranteedPellets = 1;
        private const int NumAdditionalPelletLanes = 20;
        private const int TotalPelletLanes = NumGuaranteedPellets + NumAdditionalPelletLanes;

        // Bullets fire in loose pyramid shape
        const int RowsInPyramid = 6;

        public override LoopingFrameTimer FireTimer { get; protected set; }
            = new LoopingFrameTimer(0.5f);

        // Maps loop indexes to their matching assigned lane index
        private Vector2[] LanesVelocityMap { get; set; }
        private Vector2[] LanesPositionMap { get; set; }

        private float BulletVelocityY;
        private Vector2 BulletSize;
        private float BulletOffsetX;
        private float BulletOffsetY;
        private float BulletSpreadX;
        private float BulletSpreadY;

        public FlakStrategy(FlakBullet bullet)
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

        public override PlayerBullet[] GetBullets(int weaponLevel, Vector2 playerFirePos)
        {
#if !FIREALL
            int numAdditional = 2 * (1 + PlusOneIfMaxLevel(weaponLevel));
            int numToGet = NumGuaranteedPellets + numAdditional;
#else
            int numAdditional = TotalPelletLanes - NumGuaranteedPellets;
            int numToGet = TotalPelletLanes;
#endif

            bool[] shouldFireLanes = RandomUtil.ShuffledBools(NumAdditionalPelletLanes, numAdditional);
            FlakBullet[] ret = PoolManager.Instance.BulletPool.Get<FlakBullet>(numToGet);

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

        private void FireLane(FlakBullet bullet, int laneIndex, Vector2 playerFirePos)
        {
            Vector2 newFirePos = playerFirePos + LanesPositionMap[laneIndex];
            bullet.transform.position = newFirePos;

            Vector2 newVelocity = LanesVelocityMap[laneIndex];
            bullet.Velocity = newVelocity;
        }

        private void FireGuaranteedLanes(FlakBullet[] ret, Vector2 playerFirePos)
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
            LanesPositionMap = new Vector2[TotalPelletLanes];

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
                    LanesPositionMap[laneCounter++].Set(xOffset, yOffset + yOffsetVariance);

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
            LanesVelocityMap = new Vector2[TotalPelletLanes];

            int laneCounter = 0;
            for (int y = 0; y < RowsInPyramid; y++)
            {
                float yVelocity = (y * BulletSpreadY) + BulletVelocityY;
                float xStart = y * -0.5f;
                for (int x = 0; x <= y; x++)
                {
                    float xVelocity = (xStart + x) * BulletSpreadX;
                    LanesVelocityMap[laneCounter++].Set(xVelocity, yVelocity);
                }
            }
        }
    }
}
