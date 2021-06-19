using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Enemies;

namespace Assets.Statuses
{
    public class BurningStatus : DamageOverTimeStatus
    {
        public int DamageIncrease { get; set; }
        public int MaxDamage { get; set; }

        public BurningStatus(Enemy target) : base(target)
        {

        }

        public override int GetAndUpdatePower()
        {
            Power += DamageIncrease;
            Power = Math.Min(Power, MaxDamage);

            UpdateStatusBar();

            return Power;
        }

        protected override void UpdateStatusBar()
        {
            Target.HealthBar.Ignite(this);
        }

        /// <summary>
        /// Ignites an enemy. Returns true if the ignition added
        /// damage, damage increase, or max damage.
        /// </summary>
        /// <param name="damage">The amount of damage to apply on the first tick.</param>
        /// <param name="damageIncrease">The amount of damage to add each tick.</param>
        /// <param name="maxDamage">The maximum amount of damage to allow.</param>
        /// <returns></returns>
        public bool Ignite(int damage, int damageIncrease, int maxDamage)
        {
            bool anyImprovements = false;

            if(damage > Power)
            {
                Power = damage;
                anyImprovements = true;
            }

            if(damageIncrease > DamageIncrease)
            {
                DamageIncrease = damageIncrease;
                anyImprovements = true;
            }

            if(maxDamage > MaxDamage)
            {
                MaxDamage = maxDamage;
                anyImprovements = true;
            }

            if (anyImprovements)
            {
                UpdateStatusBar();
                return true;
            }
            else
                return false;
        }

        public void IgniteOther(Enemy other)
        {
            other.Ignite(DamageIncrease, DamageIncrease, MaxDamage);
        }

        protected override void OnReset()
        {
            DamageIncrease = 0;
            MaxDamage = 0;
        }
    }
}
