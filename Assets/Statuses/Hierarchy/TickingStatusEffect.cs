using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Enemies;
using Assets.UI;
using Assets.Util;

namespace Assets.Statuses
{
    /// <summary>
    /// Represents a status effect that "ticks" once every second.
    /// </summary>
    public abstract class TickingStatusEffect
    {
        private const float ActivationInterval = 1.0f;

        protected Enemy Target { get; set; }

        protected LoopingFrameTimer Timer { get; set; }

        public int Power { get; protected set; }
        public bool IsActive => Power > 0;

        protected bool UpdateTicks(float deltaTime) => IsActive && Timer.UpdateActivates(deltaTime);

        protected abstract void UpdateStatusBar();

        public abstract int GetAndUpdatePower();

        public TickingStatusEffect(Enemy target)
        {
            Target = target;
            Timer = new LoopingFrameTimer(ActivationInterval);
        }

        protected virtual void OnReset() { }
        public void Reset()
        {
            Timer.Reset();
            Power = 0;

            OnReset();
        }
    }
}
