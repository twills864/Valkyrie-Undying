using Assets.Bullets;
using Assets.Constants;
using Assets.Util;
using Assets.Util.ObjectPooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FireStrategies
{
    public class ShotgunStrategy : FireStrategy<ShotgunBullet>
    {
        private const float ShotgunSpeedY = 8.0f;

        private const int NumGuaranteedPellets = 3;
        private const int NumAdditionalPelletLanes = GameConstants.MaxWeaponLevel + 1;
        private const int TotalPelletLanes = NumGuaranteedPellets + NumAdditionalPelletLanes;

        private const int MiddleGuaranteedLane = (TotalPelletLanes / 2);
        private const int LeftGuaranteedLane = MiddleGuaranteedLane - 2;
        private const int RightGuaranteedLane = MiddleGuaranteedLane + 2;

        public override LoopingFrameTimer DefaultFireTimer => new LoopingFrameTimer(0.75f);

        // Maps loop indexes to their matching assigned lane index
        private int[] LanesPositionMap { get; set; }

        // TODO: Pre-load these in an Init()
        private float BulletWidth;
        private float BulletSpread;

        public ShotgunStrategy()
        {
            FillAssignedLanesMap();
        }

        public override Bullet[] GetBullets(int weaponLevel, Vector2 playerFirePos)
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


            ShotgunBullet[] ret = PoolManager.Instance.BulletPool.Get<ShotgunBullet>(numToGet);

            var first = ret[0];
            BulletWidth = first.GetComponent<Renderer>().bounds.size.x + first.BulletOffset;
            BulletSpread = first.BulletSpread;

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

        private void FireLane(ShotgunBullet bullet, int laneIndex, Vector2 playerFirePos)
        {
            int mappedIndex = LanesPositionMap[laneIndex];
            float posX = mappedIndex * BulletWidth;

            Vector2 newFirePos = playerFirePos;
            newFirePos.x += posX;

            bullet.Velocity = new Vector2(mappedIndex * BulletSpread, ShotgunSpeedY);
            bullet.transform.position = newFirePos;
        }

        // Guaranteed lanes
        private void FireGuaranteedLanes(ShotgunBullet[] ret, Vector2 playerFirePos)
        {
            for(int i = 0; i < NumGuaranteedPellets; i++)
                FireLane(ret[i], i, playerFirePos);
        }




        private void FillAssignedLanesMap()
        {
            LanesPositionMap = new int[TotalPelletLanes];

            LanesPositionMap[0] = LeftGuaranteedLane - MiddleGuaranteedLane;
            LanesPositionMap[1] = MiddleGuaranteedLane - MiddleGuaranteedLane;
            LanesPositionMap[2] = RightGuaranteedLane - MiddleGuaranteedLane;

            int laneCounter = 3;
            for (int i = 0; i < TotalPelletLanes; i++)
            {
                if (i != MiddleGuaranteedLane
                    && i != LeftGuaranteedLane
                    && i != RightGuaranteedLane)
                {
                    LanesPositionMap[laneCounter] = i - MiddleGuaranteedLane;
                    laneCounter++;
                }
            }
        }
    }
}
