using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;
using UnityEngine;

namespace Assets.Pickups
{
    /// <summary>
    /// A pickup that gives the player a heavy weapon for a brief period of time.
    /// </summary>
    /// <inheritdoc/>
    public class WeaponPickup : Pickup
    {
        public int FireStrategyIndex { get; set; }

        protected override void OnPickupSpawn()
        {
            PickupSprite = GameManager.Instance.FireStrategySprite(FireStrategyIndex);
        }

        protected override void OnPickUp()
        {
            GameManager.Instance.SetFireType(FireStrategyIndex);
        }
    }
}
