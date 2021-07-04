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
    /// <summary>
    /// A pickup that grants the player the Powerup it's currently holding.
    /// </summary>
    /// <inheritdoc/>
    public class PowerupPickup : EnemyLootPickup
    {
        public Powerup TargetPowerup { get; set; }

        /// <summary>
        /// Remember the initial sprite color in case it needs to be overridden
        /// by the default powerup color.
        /// </summary>
        public Color InitialSpriteColor;
        public Color DefaultWeaponSpriteColor;

        public override Sprite InitialPickupSprite => SpriteBank.Empty;

        protected override void OnEnemyLootPickupSpawn()
        {
            if (!TargetPowerup.IsDefaultWeaponPowerup)
            {
                SpriteColor = InitialSpriteColor;
                PickupSpriteColor = InitialSpriteColor;
            }
            else
            {
                SpriteColor = DefaultWeaponSpriteColor;
                PickupSpriteColor = DefaultWeaponSpriteColor;
            }

            PickupSprite = TargetPowerup.PowerupSprite;
        }

        protected override void OnPickUp()
        {
            TargetPowerup.Level++;

            var type = TargetPowerup.GetType();
            PowerupMenu.Instance.SetLevel(type, TargetPowerup.Level);

            DebugUI.Instance.PowerupMenuPowerLevelRowSet(TargetPowerup, TargetPowerup.Level);

            NotificationManager.AddNotification(TargetPowerup.NotificationName);

            if(TargetPowerup.IsDefaultWeaponPowerup)
                GameManager.Instance.IncreaseWeaponLevel();
        }

        protected override void OnDestructorDeactivation()
        {
            TargetPowerup.CheckIn();

            if(TargetPowerup.IsDefaultWeaponPowerup)
                Director.WeaponLevelPickupDestructed();
    }
    }
}
