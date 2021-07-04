using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Util
{
    /// <summary>
    /// A countdown that activates after being checked a given number of times.
    /// </summary>
    public struct LoopingCountdown
    {
        private int Counter;
        public int ActivationInterval;

        public LoopingCountdown(int activationInterval)
        {
            Counter = 0;
            ActivationInterval = activationInterval - 1;
        }

        public bool IsReady()
        {
            bool ret = Counter == 0;
            if (!ret)
                Counter--;
            else
                Counter = ActivationInterval;

            return ret;
        }
    }
}
