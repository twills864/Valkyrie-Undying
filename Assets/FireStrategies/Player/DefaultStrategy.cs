using Assets.Bullets.PlayerBullets;
using Assets.FireStrategyManagers;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <inheritdoc/>
    public class DefaultStrategy : PlayerFireStrategy<DefaultBullet>
    {
        public static DefaultStrategy Instance { get; private set; }

        public DefaultStrategy(DefaultBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
        {
            Instance = this;
        }

        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios)
        {
            // Default ratio is 1, since the fire speed ratio is
            // fire speed compared to the default strategy.
            const float DefaultRatio = 1.0f;

            float ratio = DefaultRatio;

            // Future modifications below

            return ratio;
        }

    }
}
