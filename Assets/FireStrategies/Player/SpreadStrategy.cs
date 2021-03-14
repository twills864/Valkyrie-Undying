using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.Util;
using Assets.ObjectPooling;
using UnityEngine;
using Assets.FireStrategyManagers;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <inheritdoc/>
    public class SpreadStrategy : PlayerFireStrategy<SpreadBullet>
    {
        private const int NumGuaranteedPellets = 1;
        private const int NumBaseRandomPellets = 1;
        private const int NumAdditionalPelletLanes = GameConstants.MaxWeaponLevel + 2 + NumBaseRandomPellets;
        private const int TotalPelletLanes = NumGuaranteedPellets + NumAdditionalPelletLanes;

        private const int MiddleGuaranteedLane = (TotalPelletLanes / 2);

        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios) => ratios.Spread;

        //public override LoopingFrameTimer FireTimer { get; protected set; }
        //    = new LoopingFrameTimer(0.5f);

        // Maps loop indexes to their matching assigned lane index
        private Vector2[] LanesVelocityMap { get; set; }
        private Vector3[] LanesPositionMap { get; set; }

        private float BulletVelocityY;
        private float FireRadius;
        private float AngleBetweenLanesInDegrees;
        private float AngleBetweenLanesInRadians;
        private float DampX;
        private Vector2 BulletSize;

        public SpreadStrategy(SpreadBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
        {
            BulletVelocityY = bullet.BulletVelocityY;
            FireRadius = bullet.FireRadius;
            AngleBetweenLanesInDegrees = bullet.AngleBetweenLanesInDegrees;
            AngleBetweenLanesInRadians = Mathf.Deg2Rad * AngleBetweenLanesInDegrees;
            DampX = bullet.DampX;
            BulletSize = bullet.GetComponent<Renderer>().bounds.size;

            FillAssignedLanesMap();
        }

        public override PlayerBullet[] GetBullets(int weaponLevel, Vector3 playerFirePos)
        {
            int numToGet;
            bool[] shouldFireLanes;

            if (weaponLevel != GameConstants.MaxWeaponLevel)
            {
                numToGet = NumGuaranteedPellets + NumBaseRandomPellets + weaponLevel;
                shouldFireLanes = RandomUtil.ShuffledBools(NumAdditionalPelletLanes, weaponLevel + NumBaseRandomPellets);
            }
            else
            {
                numToGet = TotalPelletLanes;
                shouldFireLanes = LinqUtil.UniformArray(NumAdditionalPelletLanes, true);
            }

            SpreadBullet[] ret = PoolManager.Instance.BulletPool.GetMany<SpreadBullet>(numToGet);

            FireGuaranteedLanes(ret, playerFirePos);

            int nextFireIndex = NumGuaranteedPellets;

            // The spread shot always fires in lane 0;
            // The other lanes between -4 and +4 fire randomly based on weaponLevel.
            for (int i = NumGuaranteedPellets; i < TotalPelletLanes; i++)
            {
                bool shouldFireLane = shouldFireLanes[i - NumGuaranteedPellets];
                if (shouldFireLane)
                    FireLane(ret[nextFireIndex++], i, playerFirePos);
            }

            return ret;
        }

        private void FireLane(SpreadBullet bullet, int laneIndex, Vector3 playerFirePos)
        {
            Vector3 newFirePos = playerFirePos + LanesPositionMap[laneIndex];
            bullet.transform.position = newFirePos;

            Vector2 newVelocity = LanesVelocityMap[laneIndex];
            bullet.Velocity = newVelocity;

            bullet.SetDamage(laneIndex == 0);
        }

        private void FireGuaranteedLanes(SpreadBullet[] ret, Vector3 playerFirePos)
        {
            for(int i = 0; i < NumGuaranteedPellets; i++)
                FireLane(ret[i], i, playerFirePos);
        }

        private Vector3 DampenXPosition(Vector3 position)
        {
            var ret = new Vector3(position.x * DampX, position.y);
            return ret;
        }

        private void FillAssignedLanesMap()
        {
            Vector3 anchor = new Vector3(0, -FireRadius);

            LanesPositionMap = new Vector3[TotalPelletLanes];
            LanesVelocityMap = new Vector2[TotalPelletLanes];

            LanesPositionMap[0] = Vector3.zero;
            LanesVelocityMap[0] = new Vector2(0, BulletVelocityY);

            const float angleOffset = Mathf.Deg2Rad * 90;

            for (int i = 0; i < MiddleGuaranteedLane; i++)
            {
                int baseIndex = (NumGuaranteedPellets) + (i * 2);

                // Increase i by one so as to not overlap base fire
                float angle = (i+1) * AngleBetweenLanesInRadians;
                angle += angleOffset;

                var unitVector = MathUtil.Vector3AtRadianAngle(angle);

                LanesPositionMap[baseIndex] = anchor + DampenXPosition(unitVector * FireRadius);
                LanesVelocityMap[baseIndex] = unitVector * BulletVelocityY;

                var flippedUnitVector = new Vector2(-unitVector.x, unitVector.y);
                LanesPositionMap[baseIndex + 1] = anchor + DampenXPosition(flippedUnitVector * FireRadius);
                LanesVelocityMap[baseIndex + 1] = flippedUnitVector * BulletVelocityY;
            }
        }
    }
}
