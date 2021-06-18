using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.FireStrategyManagers;
using Assets.ObjectPooling;
using Assets.UI.SpriteBank;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <inheritdoc/>
    public class GatlingStrategy : PlayerFireStrategy<GatlingBullet>
    {
        private const float DefaultFireTime = 0.4f;
        private const int FireCounterMax = 3;

        protected override Sprite GetPickupSprite(HeavyWeaponSpriteBank bank) => bank.Gatling;
        public override bool UpdateOnFire => false;

        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios) => ratios.Gatling;

        //public override LoopingFrameTimer FireTimer { get; protected set; }
        //    = LoopingFrameTimer.Default();

        private LoopingCountdown CenterShot = new LoopingCountdown(FireCounterMax);

        public GatlingStrategy(GatlingBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
        {
        }

        public override PlayerBullet[] GetBullets(int weaponLevel, Vector3 playerFirePos)
        {
            PlayerBullet[] ret;

            //if (weaponLevel != GameConstants.MaxWeaponLevel)
            ret = base.GetBullets(weaponLevel, playerFirePos);
            //else
            //{
            //    ret = PoolManager.Instance.BulletPool.GetMany<GatlingBullet>(2, playerFirePos, weaponLevel);
            //    FireAdditionalBullet((GatlingBullet) ret[1]);
            //}

            GatlingBullet bullet = (GatlingBullet) ret[0];

            float offset = CalculateAngleOffset(weaponLevel);
            bullet.RayCastUp(offset);

            RecalculateActivationInterval(weaponLevel);

            return ret;
        }

        // Offset bound = AngleMax - (weaponLevel * LevelScale)
        private float CalculateAngleOffset(int weaponLevel)
        {
            const float AngleMax = 15f * Mathf.Deg2Rad;
            const float LevelScale = 2f * Mathf.Deg2Rad;

            if (CenterShot.IsReady())
                return 0;

            float bound = AngleMax - (weaponLevel * LevelScale);

            float ret = RandomUtil.Float(-bound, bound);
            return ret;
        }

        // ActivationInterval = (DefaultFireTime * numerator) / (fix + level)
        private void RecalculateActivationInterval(int weaponLevel)
        {
            const float numerator = 3f;
            const float fix = 3f;

            float ret = (numerator * DefaultFireTime) / (fix + PlusOneIfMaxLevel(weaponLevel));
            FireTimer.ActivationInterval = ret;
        }

        private void FireAdditionalBullet(GatlingBullet bullet)
        {
            const float AngleMax = 80f * Mathf.Deg2Rad;

            float minAngle;
            float maxAngle;

            if (RandomUtil.Bool())
            {
                minAngle = 0;
                maxAngle = AngleMax;
            }
            else
            {
                minAngle = Mathf.PI - AngleMax;
                maxAngle = Mathf.PI;
            }

            float angle = RandomUtil.Float(minAngle, maxAngle);

            const float WidthCompensation = 1.5f;
            bullet.RayCast(angle, WidthCompensation);
        }
    }
}
