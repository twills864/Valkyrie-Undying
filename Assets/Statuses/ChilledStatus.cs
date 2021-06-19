using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Enemies;

namespace Assets.Statuses
{
    public class ChilledStatus : CountdownStatus
    {
        public ChilledStatus(Enemy target) : base(target)
        {

        }

        protected override void UpdateStatusBar()
        {
            Target.HealthBar.AddChill(this);
        }

        public void AddChill(int time)
        {
            Power += time;
            UpdateStatusBar();
        }
    }
}
