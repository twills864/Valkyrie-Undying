using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Pickups
{
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
