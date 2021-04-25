using System.Collections.Generic;
using System.Linq;
using Assets.ColorManagers;
using Assets.Pickups;
using Assets.Powerups;
using Assets.UI;
using Assets.Util;
using UnityEngine;

namespace Assets.ObjectPooling
{
    /// <inheritdoc/>
    public class PickupPoolList : PoolList<Pickup>
    {
#pragma warning disable 0414

        [SerializeField]
        private WeaponPickup WeaponPrefab = null;

        [SerializeField]
        private PowerupPickup PowerupPrefab = null;

        [SerializeField]
        private WeaponLevelPickup WeaponLevelPrefab = null;

        [SerializeField]
        private OneUpPickup OneUpPrefab = null;

#pragma warning restore 0414

        public List<Powerup> AllAssignablePowerups { get; private set; }

        protected override Color GetDefaultColor(in ColorManager colorManager)
            => Color.white;

        protected override void OnInitSprites(in ColorManager colorManager)
        {
            WeaponPrefab.SpriteColor = colorManager.Pickup.Weapon;
            PowerupPrefab.SpriteColor = colorManager.Pickup.Powerup;
            WeaponLevelPrefab.SpriteColor = colorManager.Pickup.WeaponLevel;
            OneUpPrefab.SpriteColor = colorManager.Pickup.OneUp;


            // TODO: Load powerups, or refactor into somewhere else
        }

        public void InitializePowerups(List<Powerup> allPowerups)
        {
            // Create new List so that we can remove elements or otherwise modify this list as needed.
            AllAssignablePowerups = allPowerups.OrderByDescending(x => x.MaxLevel).ToList();
        }

        public PowerupPickup GetRandomPowerup(Vector3 position)
        {
            var powerup = RandomUtil.RandomElement(AllAssignablePowerups);
            powerup.CheckOut();

            if (powerup.AreAllPowerupsCheckedOut)
                AllAssignablePowerups.Remove(powerup);

            var pickup = Get<PowerupPickup>(position);
            pickup.TargetPowerup = powerup;

            return pickup;
        }

        /// <summary>
        /// Restores a powerup's ability to be spawned if
        /// the powerup that's about to be checked in
        /// was the last obtainable powerup.
        /// </summary>
        /// <param name="powerup">The powerup to check.</param>
        public void BeforePowerupCheckIn(Powerup powerup)
        {
            if (powerup.AreAllPowerupsCheckedOut)
                AllAssignablePowerups.Add(powerup);
        }
    }
}
