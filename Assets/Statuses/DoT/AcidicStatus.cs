using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Enemies;

namespace Assets.Statuses
{
    /// <summary>
    /// Deals a decreasing amount of damage over time.
    /// </summary>
    /// <inheritdoc/>
    public class AcidicStatus : DamageOverTimeStatus
    {
        private const int DamageDecreasePerTick = 1;

        public AcidicStatus(Enemy target) : base(target)
        {

        }

        public override int GetAndUpdatePower()
        {
            int powerBefore = Power;
            Power -= DamageDecreasePerTick;
            UpdateStatusBar();

            return powerBefore;
        }

        protected override void UpdateStatusBar()
        {
            Target.HealthBar.AddAcid(this);
        }

        /// <summary>
        /// Adds acid to an enemy.
        /// </summary>
        /// <param name="damage">The amount of damage to apply on the first tick.</param>
        public void AddAcid(int damage)
        {
            if(damage > Power)
            {
                Power = damage;
                UpdateStatusBar();
            }
        }
    }
}
