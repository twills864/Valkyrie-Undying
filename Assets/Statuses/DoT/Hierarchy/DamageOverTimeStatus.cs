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
    /// Deals a specified amount of damage to the affected enemy each tick.
    /// </summary>
    public abstract class DamageOverTimeStatus : TickingStatusEffect
    {
        private const float ActivationInterval = 1.0f;

        public bool UpdateKills(float deltaTime)
        {
            if (UpdateTicks(deltaTime))
            {
                bool kills = Target.StatusDamageKills(this);
                return kills;
            }
            else
                return false;
        }

        public DamageOverTimeStatus(Enemy target) : base(target)
        {
        }
    }
}
