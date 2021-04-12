using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.Powerups;
using Assets.UI.PowerupMenu;
using Assets.Util;
using UnityEngine;

namespace Assets.Pickups
{
    public class PowerupPickup : Pickup
    {
        public Powerup TargetPowerup { get; set; }

        protected override void OnPickUp()
        {
            GameManager.Instance.CreateFleetingText(TargetPowerup.PowerupName, SpaceUtil.WorldMap.Center);

            TargetPowerup.Level++;

            var type = TargetPowerup.GetType();
            PowerupMenu.Instance.SetLevel(type, TargetPowerup.Level);

            DebugUI.Instance.PowerupMenuPowerLevelRowSet(TargetPowerup, TargetPowerup.Level);
        }
    }
}
