//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Assets.Util
//{
//    /// <inheritdoc/>
//    public class EaseInFrameTimer : FrameTimer
//    {
//        public EaseInFrameTimer(float activationInterval) : base(activationInterval)
//        {

//        }

//        public override void Increment(float deltaTime)
//        {
//            if (!Activated)
//            {
//                Elapsed += deltaTime;

//                if (Elapsed >= ActivationInterval)
//                {
//                    OverflowDeltaTime = Elapsed - ActivationInterval;
//                    Elapsed = ActivationInterval;
//                    Activated = true;
//                }
//            }
//        }


//        protected float ModifyCompletionRatio(float currentRatioComplete)
//        {
//            return AdjustRatio(currentRatioComplete);
//        }

//        public static float AdjustRatio(float currentRatioComplete)
//        {
//            float ret = currentRatioComplete * (2 - currentRatioComplete);
//            return ret;
//        }
//    }
//}
