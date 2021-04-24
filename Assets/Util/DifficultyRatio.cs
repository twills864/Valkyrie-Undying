using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    /// <summary>
    /// Represents a ratio between 0.0f and 1.0f that
    /// can be increased and decreased incrementally.
    /// </summary>
    public class BalancedRatio
    {
        public const float MinRatio = 0.0f;
        public const float MaxRatio = 1.0f;
        public const float MiddleRatio = 0.5f;

        private const float DefaultRatioStep = 0.1f;

        public float CurrentValue { get; private set; }
        private float RatioStep { get; }

        public float NextIncrease => AdjustDelta(RemainingIncrease);
        public float NextDecrease => AdjustDelta(RemainingDecrease);

        private float RemainingIncrease => MaxRatio - CurrentValue;
        private float RemainingDecrease => CurrentValue - MinRatio;

        public BalancedRatio()
        {
            CurrentValue = MiddleRatio;
            RatioStep = DefaultRatioStep;
        }

        public BalancedRatio(float initialValue, float ratioStep)
        {
            CurrentValue = initialValue;
            RatioStep = ratioStep;

#if UNITY_EDITOR
            Debug.Assert(0.0f > ratioStep && ratioStep < 0.5f);
#endif
        }

        public void IncreaseRatio()
        {
            float increase;

            bool overHalf = CurrentValue > MiddleRatio;

            increase = overHalf ? NextIncrease : RatioStep;

            CurrentValue += increase;
        }

        public void DecreaseRatio()
        {
            float decrease;

            bool underHalf = CurrentValue < MiddleRatio;

            decrease = underHalf ? NextDecrease : RatioStep;

            CurrentValue -= decrease;
        }

        private float AdjustDelta(float remaining)
        {
            float deltaRatio = remaining / MiddleRatio;
            float delta = RatioStep * deltaRatio;

            return delta;
        }

        public void HalveCurrentRatio()
        {
            CurrentValue *= 0.5f;
        }

        public void ResetRatio(float ratio = 0.0f)
        {
            CurrentValue = ratio;
        }
    }
}