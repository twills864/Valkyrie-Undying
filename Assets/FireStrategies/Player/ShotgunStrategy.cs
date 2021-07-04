using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Util;
using Assets.ObjectPooling;
using UnityEngine;
using Assets.FireStrategyManagers;
using Assets.UI.SpriteBank;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <summary>
    /// Fires a spread of Shotgun bullets in a rough cone shape.
    /// The number of bullets in the spread increases with the bullet level.
    /// </summary>
    /// <inheritdoc/>
    public class ShotgunStrategy : PlayerFireStrategy<ShotgunBullet>
    {
        private const int NumGuaranteedPellets = 3;
        private const int NumAdditionalPelletLanes = GameConstants.MaxWeaponLevel + 1;
        private const int TotalPelletLanes = NumGuaranteedPellets + NumAdditionalPelletLanes;

        private const int MiddleGuaranteedLane = (TotalPelletLanes / 2);
        private const int LeftGuaranteedLane = MiddleGuaranteedLane - 2;
        private const int RightGuaranteedLane = MiddleGuaranteedLane + 2;

        protected override Sprite GetPickupSprite(HeavyWeaponSpriteBank bank) => bank.Shotgun;

        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios) => ratios.Shotgun;

        //public override LoopingFrameTimer FireTimer { get; protected set; }
        //    = new LoopingFrameTimer(0.75f);

        // Maps loop indexes to their matching assigned lane index
        private FireLane[] FireLanes { get; } = new FireLane[TotalPelletLanes];

        private float BulletVelocityY;
        private Vector2 BulletSize;
        private float BulletOffsetX;
        private float BulletOffsetY;
        private float BulletSpreadX;
        private float BulletSpreadY;

        public ShotgunStrategy(ShotgunBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
        {
            BulletVelocityY = bullet.BulletVelocityY;
            BulletSize = bullet.GetComponent<Renderer>().bounds.size;
            BulletOffsetX = bullet.BulletOffsetX;
            BulletOffsetY = bullet.BulletOffsetY;
            BulletSpreadX = bullet.BulletSpreadX;
            BulletSpreadY = bullet.BulletSpreadY;

            FillAssignedLanesMap();
        }

        public override PlayerBullet[] GetBullets(int weaponLevel, Vector3 playerFirePos)
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


            ShotgunBullet[] ret = PoolManager.Instance.BulletPool.GetMany<ShotgunBullet>(numToGet);

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

        private void FireLane(ShotgunBullet bullet, int laneIndex, Vector3 playerFirePos)
        {
            var lane = FireLanes[laneIndex];
            lane.ApplyToSprite(bullet, playerFirePos);
        }

        // Guaranteed lanes
        private void FireGuaranteedLanes(ShotgunBullet[] ret, Vector3 playerFirePos)
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

                var offset = new Vector3(newIndex * (BulletOffsetX + BulletSize.x),
                    isEvenPositionYOffsets[isEvenIndex]);
                var velocity = new Vector2(newIndex * BulletSpreadX,
                    isEvenVelocityYOffsets[isEvenIndex]);

                FireLanes[i] = new FireLane(offset, velocity);
            }
        }
    }
}
