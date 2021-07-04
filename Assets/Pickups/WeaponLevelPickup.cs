using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Pickups
{
    /// <summary>
    /// A pickup that grants the player an extra heavy weapon bullet level.
    /// </summary>
    /// <inheritdoc/>
    [Obsolete("Weapon levels are now granted by PowerupPickups that hold DefaultWeaponPowerups.")]
    public class WeaponLevelPickup : EnemyLootPickup
    {
        public override Sprite InitialPickupSprite => SpriteBank.HeavyWeaponLevelUp;

        protected override void OnPickUp()
        {
            GameManager.Instance.IncreaseWeaponLevel();
        }

        protected override void OnDestructorDeactivation()
        {
            Director.WeaponLevelPickupDestructed();
        }
    }
}
