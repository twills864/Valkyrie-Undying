using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Enemies;

namespace Assets.Statuses
{
    /// <summary>
    /// Deals a constant amount of damage over time.
    /// </summary>
    /// <inheritdoc/>
    public class PoisonedStatus : DamageOverTimeStatus
    {
        public PoisonedStatus(Enemy target) : base(target)
        {

        }

        // Poison damage is constant.
        public override int GetAndUpdatePower() => Power;

        protected override void UpdateStatusBar()
        {
            Target.HealthBar.AddPoison(this);
        }

        public void AddPoison(int damage)
        {
            if (damage > Power)
            {
                Power = damage;
                UpdateStatusBar();
            }
        }
    }
}
