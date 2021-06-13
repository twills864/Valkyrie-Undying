using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Pickups
{
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
