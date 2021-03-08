using Assets.Bullets.PlayerBullets;
using Assets.Util;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <summary>
    /// Two-step fire strategy:
    /// 1. Spawn a Battle-Frenzied Guillotine spawner that acts as an outline for the laser about to fire.
    /// 2. Fire a Battle-Frenzied Guillotine.
    /// </summary>
    /// <inheritdoc/>
    public class BfgStrategy : PlayerFireStrategy<BfgBullet>
    {
        public override LoopingFrameTimer FireTimer { get; protected set; }
            = new LoopingFrameTimer(1f);

        public BfgStrategy(BfgBullet bullet) : base(bullet)
        {
        }
    }
}
