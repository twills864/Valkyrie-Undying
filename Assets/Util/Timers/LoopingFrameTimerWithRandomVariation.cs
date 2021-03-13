using System.Diagnostics;

namespace Assets.Util
{
    /// <inheritdoc/>
    [DebuggerDisplay("{DebuggerDisplay}")]
    public class LoopingFrameTimerWithRandomVariation : LoopingFrameTimer
    {
        protected float Variance { get; }

        public override LoopingFrameTimer CreateClone()
        {
            float newActivationInterval = ActivationInterval + Variance;
            return new LoopingFrameTimerWithRandomVariation(newActivationInterval, Variance);
        }

        public LoopingFrameTimerWithRandomVariation(float activationInterval, float plusOrMinusVariance)
            :base(activationInterval - plusOrMinusVariance)
        {
            Variance = plusOrMinusVariance * 2;
            Elapsed += RandomUtil.Float(Variance);
        }

        public override void Increment(float deltaTime)
        {
            Elapsed += deltaTime;

            if (Elapsed < ActivationInterval)
            {
                Activated = false;
                OverflowDeltaTime = 0f;
            }
            else
            {
                Activated = true;
                OverflowDeltaTime = Elapsed - ActivationInterval;
                Elapsed = OverflowDeltaTime - RandomUtil.Float(Variance);
            }
        }

        protected override string DebuggerDisplay
        {
            get
            {
                float activationInterval = ActivationInterval + (Variance * 0.5f);
                var ret = $"[{activationInterval.ToString("0.##")} ±{Variance.ToString("0.##")}] {base.DebuggerDisplay}";
                return ret;
            }
        }
    }
}
