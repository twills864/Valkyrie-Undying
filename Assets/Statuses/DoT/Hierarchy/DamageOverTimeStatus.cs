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
    public abstract class DamageOverTimeStatus
    {
        private const float ActivationInterval = 1.0f;

        protected Enemy Target { get; set; }

        protected LoopingFrameTimer Timer { get; set; }

        public int Damage { get; protected set; }
        public bool IsActive => Damage > 0;

        public bool UpdateKills(float deltaTime)
        {
            if (IsActive && Timer.UpdateActivates(deltaTime))
            {
                bool kills = Target.StatusDamageKills(this);
                return kills;
            }
            else
                return false;
        }

        protected abstract void UpdateStatusBar();

        public abstract int GetAndUpdateDamage();

        public DamageOverTimeStatus(Enemy target)
        {
            Target = target;
            Timer = new LoopingFrameTimer(ActivationInterval);
        }

        protected virtual void OnReset() { }
        public void Reset()
        {
            Timer.Reset();
            Damage = 0;

            OnReset();
        }
    }
}
