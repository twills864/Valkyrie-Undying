using Assets.Bullets.PlayerBullets;
using Assets.FireStrategyManagers;
using Assets.Util;

namespace Assets.FireStrategies.PlayerFireStrategies
{
    /// <inheritdoc/>
    public class BasicStrategy : PlayerFireStrategy<BasicBullet>
    {
        public BasicStrategy(BasicBullet bullet, in PlayerFireStrategyManager manager) : base(bullet, manager)
        {
        }

        protected override float GetFireSpeedRatio(in PlayerFireStrategyManager.PlayerRatio ratios) => 1.0f;

        //public override LoopingFrameTimer FireTimer { get; protected set; }
        //    = new LoopingFrameTimer(0.5f);

    }
}
