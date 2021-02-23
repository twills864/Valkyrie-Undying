using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.GameTasks;

namespace Assets.Util
{
    public class EaseOutTimer : TimeModifyingFrameTimer
    {
        public EaseOutTimer(float activationInterval) : base(activationInterval)
        {
        }

        protected override float ModifyCompletionRatio(float currentRatioComplete)
        {
            float ret = EaseOut.AdjustRatio(currentRatioComplete);
            return ret;
        }

        protected override float InverseRatio(float currentRatioComplete)
        {
            return EaseOut.InverseRatio(currentRatioComplete);
        }

        public override float RatioRemaining
        {
            get => base.RatioRemaining;
            set => throw new NotImplementedException();
        }
    }
}
