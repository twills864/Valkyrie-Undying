using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Pickups
{
    public class WeaponLevelPickup : Pickup
    {
        protected override void OnPickUp()
        {
            GameManager.Instance.IncreaseWeaponLevel();
        }
    }
}
