using System;
using Assets.Bullets.PlayerBullets;
using Assets.Constants;
using Assets.FireStrategies.PlayerFireStrategies;
using Assets.ObjectPooling;
using Assets.Powerups.Balance;
using Assets.UI.SpriteBank;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups
{
    /// <summary>
    /// Splits the player's default weapon into two separate shots
    /// that fire in a V-shaped pattern.
    /// </summary>
    /// <inheritdoc/>
    [Obsolete(ObsoleteConstants.FollowTheFun)]
    public class SnakeBitePowerup : OnDefaultWeaponFirePowerup
    {
        public override int MaxLevel => 1;
        protected override Sprite GetPowerupSprite(PowerupSpriteBank bank) => bank.SnakeBite;

        private int PoisonDamage { get; set; }
        public float FireSpeedRatio { get; private set; }

        private Vector2 SpeedScaleLeft { get; set; }
        private Vector2 SpeedScaleRight => SpeedScaleLeft.ScaleX(-1f);

        protected override void InitBalance(in PowerupBalanceManager.OnDefaultWeaponFireBalance balance)
        {
            PoisonDamage = balance.SnakeBite.PoisonDamage;
            FireSpeedRatio = balance.SnakeBite.FireSpeedRatio;

            float angleOffset = balance.SnakeBite.Angle * Mathf.Deg2Rad;

            const float AngleUp = MathUtil.PiHalf;
            float angleLeft = AngleUp + angleOffset;

            SpeedScaleLeft = MathUtil.Vector2AtRadianAngle(angleLeft, 1.0f);
        }

        public override void OnLevelUp()
        {
            DefaultStrategy.ApplySnakeBite(this);
        }

        public override void OnFire(Vector3 position, DefaultBullet[] bullets)
        {
            const int IndexLeft = 0;
            const int IndexRight = 1;

            float spawnOffset = bullets[0].ColliderMap.Width;

            InitBullet(bullets[IndexLeft], SpeedScaleLeft, -spawnOffset);
            InitBullet(bullets[IndexRight], SpeedScaleRight, spawnOffset);
        }

        private void InitBullet(DefaultBullet bullet, Vector2 velocityScale, float spawnOffsetX)
        {
            bullet.Velocity = bullet.VelocityY * velocityScale;
            bullet.SnakeBiteDamage += PoisonDamage;
            bullet.PositionX += spawnOffsetX;
        }
    }
}
