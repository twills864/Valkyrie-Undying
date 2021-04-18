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

        public Vector2 Size { get; private set; }

        protected override void OnPickupInit()
        {
            var renderer = GetComponent<Renderer>();
            Size = renderer.bounds.size;
        }

        protected override void OnPickUp()
        {
            GameManager.Instance.SetFireType(FireStrategyIndex);
        }

        protected override void OnDeactivate()
        {
            GameManager.Instance.ResetWeaponRainTimer();
        }
    }
}
