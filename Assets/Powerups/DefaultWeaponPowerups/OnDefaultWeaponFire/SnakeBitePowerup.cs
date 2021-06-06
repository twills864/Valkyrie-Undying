using Assets.Bullets.PlayerBullets;
using Assets.FireStrategies.PlayerFireStrategies;
using Assets.Powerups.Balance;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Splits the player's default weapon into two separate shots
    /// that fire in a V-shaped pattern.
    /// </summary>
    /// <inheritdoc/>
    public class SnakeBitePowerup : OnDefaultWeaponFirePowerup
    {
        private const int NumBullets = 2;

        public override int MaxLevel => 1;

        private int ExtraDamage { get; set; }

        private Vector2 SpeedScaleLeft { get; set; }
        private Vector2 SpeedScaleRight => VectorUtil.ScaleX2(SpeedScaleLeft, -1f);

        protected override void InitBalance(in PowerupBalanceManager.OnDefaultWeaponFireBalance balance)
        {
            ExtraDamage = balance.SnakeBite.DamageIncrease;

            float angleOffset = balance.SnakeBite.Angle * Mathf.Deg2Rad;

            const float AngleUp = MathUtil.PiHalf;
            float angleLeft = AngleUp + angleOffset;

            SpeedScaleLeft = MathUtil.Vector2AtRadianAngle(angleLeft, 1.0f);
        }

        public override void OnLevelUp()
        {
            DefaultStrategy.NumBulletsToGet = NumBullets;
        }

        public override void OnFire(Vector3 position, DefaultBullet[] bullets)
        {
            const int IndexLeft = 0;
            const int IndexRight = 1;

            InitBullet(bullets[IndexLeft], SpeedScaleLeft);
            InitBullet(bullets[IndexRight], SpeedScaleRight);
        }

        private void InitBullet(DefaultBullet bullet, Vector2 velocityScale)
        {
            bullet.Velocity = bullet.InitialSpeed * velocityScale;
            bullet.SnakeBiteDamage += ExtraDamage;
        }
    }
}
