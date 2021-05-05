using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;
using UnityEngine;

namespace Assets.Pickups
{
    public class WeaponPickup : Pickup
    {
        public int FireStrategyIndex { get; set; }

        protected override void OnPickUp()
        {
            GameManager.Instance.SetFireType(FireStrategyIndex);
        }
    }
}
