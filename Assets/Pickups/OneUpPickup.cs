using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.UI;
using UnityEngine;

namespace Assets.Pickups
{
    /// <summary>
    /// A pickup that grants the player an extra life.
    /// </summary>
    /// <inheritdoc/>
    public class OneUpPickup : Pickup
    {
        public override Sprite InitialPickupSprite => SpriteBank.OneUp;

        protected override void OnPickUp()
        {
            GameManager.Instance.LivesLeft++;
            NotificationManager.AddNotification("One up!");
        }
    }
}
