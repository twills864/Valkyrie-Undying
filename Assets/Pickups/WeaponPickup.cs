using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Pickups
{
    public class WeaponPickup : Pickup
    {
        protected override void OnPickUp()
        {
            Player.Instance.LocalScaleX *= 0.9f;
        }
    }
}
