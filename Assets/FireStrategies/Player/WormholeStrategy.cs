using Assets.Bullets.PlayerBullets;
using Assets.FireStrategyManagers;
using Assets.UI.SpriteBank;
using Assets.Util;
using UnityEngine;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <inheritdoc/>
    public class WormholeStrategy : PlayerFireStrategy<WormholeBullet>
    {
        protected override Sprite GetPickupSprite(HeavyWeaponSpriteBank bank) => bank.Wormhole;

        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios) => ratios.Wormhole;

        //public override LoopingFrameTimer FireTimer { get; protected set; }
        //    = new LoopingFrameTimer(0.9f);

        public WormholeStrategy(WormholeBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
        {
        }
    }
}
