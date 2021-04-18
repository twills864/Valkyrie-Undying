using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.GameTasks;
using Assets.Powerups;
using Assets.UI;
using Assets.UI.PowerupMenu;
using Assets.Util;
using UnityEngine;

namespace Assets.Pickups
{
    public class PowerupPickup : EnemyLootPickup
    {
        public Powerup TargetPowerup { get; set; }

        protected override void OnPickUp()
        {

            TargetPowerup.Level++;

            var type = TargetPowerup.GetType();
            PowerupMenu.Instance.SetLevel(type, TargetPowerup.Level);

            DebugUI.Instance.PowerupMenuPowerLevelRowSet(TargetPowerup, TargetPowerup.Level);

            NotificationManager.AddNotification(TargetPowerup.NotificationName);
        }
    }
}
