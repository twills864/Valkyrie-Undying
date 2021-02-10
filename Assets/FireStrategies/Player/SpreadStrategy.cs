using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Util;
using Assets.Util.ObjectPooling;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <inheritdoc/>
    public class SpreadStrategy : PlayerFireStrategy<SpreadBullet>
    {
        private const int NumGuaranteedPellets = 3;
        private const int NumAdditionalPelletLanes = GameConstants.MaxWeaponLevel + 1;
        private const int TotalPelletLanes = NumGuaranteedPellets + NumAdditionalPelletLanes;

        private const int MiddleGuaranteedLane = (TotalPelletLanes / 2);
        private const int LeftGuaranteedLane = MiddleGuaranteedLane - 2;
        private const int RightGuaranteedLane = MiddleGuaranteedLane + 2;

        public override LoopingFrameTimer FireTimer { get; protected set; }
            = new LoopingFrameTimer(0.75f);

        // Maps loop indexes to their matching assigned lane index
        private Vector2[] LanesVelocityMap { get; set; }
        private Vector2[] LanesPositionMap { get; set; }

        private float BulletVelocityY;
        private Vector2 BulletSize;
        private float BulletOffsetX;
        private float BulletOffsetY;
        private float BulletSpreadX;
        private float BulletSpreadY;

        public SpreadStrategy(SpreadBullet bullet)
        {
            BulletVelocityY = bullet.BulletVelocityY;
            BulletSize = bullet.GetComponent<Renderer>().bounds.size;
            BulletOffsetX = bullet.BulletOffsetX;
            BulletOffsetY = bullet.BulletOffsetY;
            BulletSpreadX = bullet.BulletSpreadX;
            BulletSpreadY = bullet.BulletSpreadY;

            FillAssignedLanesMap();
        }

        public override PlayerBullet[] GetBullets(int weaponLevel, Vector2 playerFirePos)
        {
            int numToGet;
            bool[] shouldFireLanes;

            if (weaponLevel == 0)
            {
                numToGet = NumGuaranteedPellets;
                shouldFireLanes = new bool[NumAdditionalPelletLanes];
            }
            else if (weaponLevel == GameConstants.MaxWeaponLevel)
            {
                numToGet = TotalPelletLanes;
                shouldFireLanes = LinqUtil.UniformArray(NumAdditionalPelletLanes, true);
            }
            else
            {
                numToGet = NumGuaranteedPellets + weaponLevel;
                shouldFireLanes = RandomUtil.ShuffledBools(NumAdditionalPelletLanes, weaponLevel);
            }


            SpreadBullet[] ret = PoolManager.Instance.BulletPool.Get<SpreadBullet>(numToGet);

            FireGuaranteedLanes(ret, playerFirePos);

            int nextFireIndex = NumGuaranteedPellets;
            if (weaponLevel > 0)
            {
                // The shotgun always fires in lanes -2, 0, and +2;
                // The other lanes between -4 and +4 fire randomly based on weaponLevel.
                for (int i = NumGuaranteedPellets; i < TotalPelletLanes; i++)
                {
                    bool shouldFireLane = shouldFireLanes[i - NumGuaranteedPellets];
                    if (shouldFireLane)
                        FireLane(ret[nextFireIndex++], i, playerFirePos);
                }
            }

            return ret;
        }

        private void FireLane(SpreadBullet bullet, int laneIndex, Vector2 playerFirePos)
        {
            Vector2 newFirePos = playerFirePos + LanesPositionMap[laneIndex];
            bullet.transform.position = newFirePos;

            Vector2 newVelocity = LanesVelocityMap[laneIndex];
            bullet.Velocity = newVelocity;
        }

        // Guaranteed lanes
        private void FireGuaranteedLanes(SpreadBullet[] ret, Vector2 playerFirePos)
        {
            for(int i = 0; i < NumGuaranteedPellets; i++)
                FireLane(ret[i], i, playerFirePos);
        }



        private void FillAssignedLanesMap()
        {
            int[] lanesIndexOffsetMapMap = new int[TotalPelletLanes];

            lanesIndexOffsetMapMap[0] = LeftGuaranteedLane - MiddleGuaranteedLane;
            lanesIndexOffsetMapMap[1] = MiddleGuaranteedLane - MiddleGuaranteedLane;
            lanesIndexOffsetMapMap[2] = RightGuaranteedLane - MiddleGuaranteedLane;

            int laneCounter = 3;
            for (int i = 0; i < TotalPelletLanes; i++)
            {
                if (i != MiddleGuaranteedLane
                    && i != LeftGuaranteedLane
                    && i != RightGuaranteedLane)
                {
                    lanesIndexOffsetMapMap[laneCounter] = i - MiddleGuaranteedLane;
                    laneCounter++;
                }
            }


            LanesPositionMap = new Vector2[TotalPelletLanes];
            LanesVelocityMap = new Vector2[TotalPelletLanes];

            float[] isEvenPositionYOffsets = new float[2]
            {
                // Odd - Some Y offset
                BulletOffsetY + BulletSize.y,

                // Even - no Y offset
                0
            };

            float[] isEvenVelocityYOffsets = new float[2]
{
                // Odd - Some Y spread
                BulletVelocityY + BulletSpreadY,

                // Even - no Y spread
                BulletVelocityY
            };

            for (int i = 0; i < TotalPelletLanes; i++)
            {
                int newIndex = lanesIndexOffsetMapMap[i];
                int isEvenIndex = MathUtil.IsEven(newIndex) ? 1 : 0;
                LanesPositionMap[i] = new Vector2(newIndex * (BulletOffsetX + BulletSize.x),
                    isEvenPositionYOffsets[isEvenIndex]);
                LanesVelocityMap[i] = new Vector2(newIndex * BulletSpreadX,
                    isEvenVelocityYOffsets[isEvenIndex]);
            }
        }
    }
}
