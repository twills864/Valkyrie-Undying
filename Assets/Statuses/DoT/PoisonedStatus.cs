using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Enemies;

namespace Assets.Statuses
{
    public class PoisonedStatus : DamageOverTimeStatus
    {
        public PoisonedStatus(Enemy target) : base(target)
        {

        }

        // Poison damage is constant.
        public override int GetAndUpdateDamage() => Damage;

        protected override void UpdateStatusBar()
        {
            Target.HealthBar.AddPoison(this);
        }

        public void AddPoison(int damage)
        {
            if (damage > Damage)
            {
                Damage = damage;
                UpdateStatusBar();
            }
        }
    }
}
