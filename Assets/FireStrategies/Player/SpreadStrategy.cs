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
    public class SpreadStrategy : PlayerFireStrategy<SpreadBullet>
    {
        private const int NumGuaranteedPellets = 1;
        private const int NumBaseRandomPellets = 1;
        private const int NumAdditionalPelletLanes = GameConstants.MaxWeaponLevel + 2 + NumBaseRandomPellets;
        private const int TotalPelletLanes = NumGuaranteedPellets + NumAdditionalPelletLanes;

        private const int MiddleGuaranteedLane = (TotalPelletLanes / 2);

        protected override Sprite GetPickupSprite(HeavyWeaponSpriteBank bank) => bank.Spread;

        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios) => ratios.Spread;

        //public override LoopingFrameTimer FireTimer { get; protected set; }
        //    = new LoopingFrameTimer(0.5f);

        // Maps loop indexes to their matching assigned lane index
        private FireLane[] FireLanes { get; } = new FireLane[TotalPelletLanes];

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
            var lane = FireLanes[laneIndex];
            lane.ApplyToSprite(bullet, playerFirePos);

            bool isMainBullet = laneIndex == 0;
            bullet.SetDamage(isMainBullet);
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

            Vector2 velocity0 = new Vector2(0, BulletVelocityY);
            FireLanes[0] = new FireLane(Vector3.zero, velocity0);

            const float angleOffset = Mathf.Deg2Rad * 90;

            for (int i = 0; i < MiddleGuaranteedLane; i++)
            {
                int baseIndex = (NumGuaranteedPellets) + (i * 2);

                // Increase i by one so as to not overlap base fire
                float angle = (i+1) * AngleBetweenLanesInRadians;
                angle += angleOffset;

                var unitVector = MathUtil.Vector3AtRadianAngle(angle);
                var basePosition = anchor + DampenXPosition(unitVector * FireRadius);
                var baseVelocity = unitVector * BulletVelocityY;
                FireLanes[baseIndex] = new FireLane(basePosition, baseVelocity);

                var flippedUnitVector = new Vector2(-unitVector.x, unitVector.y);
                var flippedPosition = anchor + DampenXPosition(flippedUnitVector * FireRadius);
                var flippedVelocity = flippedUnitVector * BulletVelocityY;
                FireLanes[baseIndex + 1] = new FireLane(flippedPosition, flippedVelocity);
            }
        }
    }
}
