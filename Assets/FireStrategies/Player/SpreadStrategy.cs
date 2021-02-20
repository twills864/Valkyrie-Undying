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
        private const int NumGuaranteedPellets = 1;
        private const int NumBaseRandomPellets = 1;
        private const int NumAdditionalPelletLanes = GameConstants.MaxWeaponLevel + 2 + NumBaseRandomPellets;
        private const int TotalPelletLanes = NumGuaranteedPellets + NumAdditionalPelletLanes;

        private const int MiddleGuaranteedLane = (TotalPelletLanes / 2);

        public override LoopingFrameTimer FireTimer { get; protected set; }
            = new LoopingFrameTimer(0.5f);

        // Maps loop indexes to their matching assigned lane index
        private Vector2[] LanesVelocityMap { get; set; }
        private Vector2[] LanesPositionMap { get; set; }

        private float BulletVelocityY;
        private float FireRadius;
        private float AngleBetweenLanesInDegrees;
        private float AngleBetweenLanesInRadians;
        private float DampX;
        private Vector2 BulletSize;

        public SpreadStrategy(SpreadBullet bullet) : base(bullet)
        {
            BulletVelocityY = bullet.BulletVelocityY;
            FireRadius = bullet.FireRadius;
            AngleBetweenLanesInDegrees = bullet.AngleBetweenLanesInDegrees;
            AngleBetweenLanesInRadians = Mathf.Deg2Rad * AngleBetweenLanesInDegrees;
            DampX = bullet.DampX;
            BulletSize = bullet.GetComponent<Renderer>().bounds.size;

            FillAssignedLanesMap();
        }

        public override PlayerBullet[] GetBullets(int weaponLevel, Vector2 playerFirePos)
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

        private void FireLane(SpreadBullet bullet, int laneIndex, Vector2 playerFirePos)
        {
            Vector2 newFirePos = playerFirePos + LanesPositionMap[laneIndex];
            bullet.transform.position = newFirePos;

            Vector2 newVelocity = LanesVelocityMap[laneIndex];
            bullet.Velocity = newVelocity;

            bullet.SetDamage(laneIndex == 0);
        }

        private void FireGuaranteedLanes(SpreadBullet[] ret, Vector2 playerFirePos)
        {
            for(int i = 0; i < NumGuaranteedPellets; i++)
                FireLane(ret[i], i, playerFirePos);
        }

        private Vector2 DampenXPosition(Vector2 vector)
        {
            var ret = new Vector2(vector.x * DampX, vector.y);
            return ret;
        }

        private void FillAssignedLanesMap()
        {
            Vector2 anchor = new Vector2(0, -FireRadius);

            LanesPositionMap = new Vector2[TotalPelletLanes];
            LanesVelocityMap = new Vector2[TotalPelletLanes];

            LanesPositionMap[0] = Vector2.zero;
            LanesVelocityMap[0] = new Vector2(0, BulletVelocityY);

            const float angleOffset = Mathf.Deg2Rad * 90;

            for (int i = 0; i < MiddleGuaranteedLane; i++)
            {
                int baseIndex = (NumGuaranteedPellets) + (i * 2);

                // Increase i by one so as to not overlap base fire
                float angle = (i+1) * AngleBetweenLanesInRadians;
                angle += angleOffset;

                var unitVector = MathUtil.VectorAtAngle(angle);

                LanesPositionMap[baseIndex] = anchor + DampenXPosition(unitVector * FireRadius);
                LanesVelocityMap[baseIndex] = unitVector * BulletVelocityY;

                var flippedUnitVector = new Vector2(-unitVector.x, unitVector.y);
                LanesPositionMap[baseIndex + 1] = anchor + DampenXPosition(flippedUnitVector * FireRadius);
                LanesVelocityMap[baseIndex + 1] = flippedUnitVector * BulletVelocityY;
            }
        }
    }
}
