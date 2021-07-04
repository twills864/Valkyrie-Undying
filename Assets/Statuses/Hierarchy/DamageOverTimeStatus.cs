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
    /// Represents a status effect that's active for a set amount of time.
    /// It will count down each second, and deactivate when its power is 0.
    /// </summary>
    /// <inheritdoc/>
    public abstract class CountdownStatus : TickingStatusEffect
    {
        public void Update(float deltaTime)
        {
            if (UpdateTicks(deltaTime))
                GetAndUpdatePower();
        }

        public CountdownStatus(Enemy target) : base(target)
        {
        }

        // Counts down each tick.
        public override int GetAndUpdatePower()
        {
            Power--;
            UpdateStatusBar();
            return Power;
        }
    }
}
