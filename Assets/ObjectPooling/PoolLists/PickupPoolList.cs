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

        public List<BasicPowerup> AssignableBasicPowerups { get; private set; }
        public List<DefaultWeaponPowerup> AssignableDefaultWeaponPowerups { get; private set; }

        protected override Color GetDefaultColor(in ColorManager colorManager)
            => Color.white;

        protected override void OnInitSprites(in ColorManager colorManager)
        {
            WeaponPrefab.SpriteColor = colorManager.Pickup.Weapon;
            WeaponLevelPrefab.SpriteColor = colorManager.Pickup.WeaponLevel;
            OneUpPrefab.SpriteColor = colorManager.Pickup.OneUp;

            PowerupPrefab.SpriteColor = colorManager.Pickup.Powerup;
            PowerupPrefab.InitialSpriteColor = colorManager.Pickup.Powerup;
            PowerupPrefab.DefaultWeaponSpriteColor = colorManager.Pickup.DefaultWeaponPowerup;

            // TODO: Load powerups, or refactor into somewhere else
        }

        public void InitializePowerups(List<Powerup> allPowerups)
        {
            var ordered = allPowerups.OrderByDescending(x => x.MaxLevel);

            AssignableBasicPowerups = ordered.Where(x => !x.IsDefaultWeaponPowerup)
                .Select(x => (BasicPowerup)x).ToList();

            AssignableDefaultWeaponPowerups = ordered.Where(x => x.IsDefaultWeaponPowerup)
                .Select(x => (DefaultWeaponPowerup)x).ToList();
        }

        public PowerupPickup GetRandomPowerup(Vector3 position, float defaultWeaponPowerupOverrideChance)
        {
            var powerup = SelectPowerup(defaultWeaponPowerupOverrideChance);
            powerup.CheckOut();

            if (powerup.AreAllPowerupsCheckedOut)
            {
                if (powerup is BasicPowerup basic)
                    AssignableBasicPowerups.Remove(basic);
                else if (powerup is DefaultWeaponPowerup defaultWeapon)
                    AssignableDefaultWeaponPowerups.Remove(defaultWeapon);
                else
                    throw ExceptionUtil.ArgumentException(() => powerup);
            }

            var pickup = Get<PowerupPickup>(position);
            pickup.TargetPowerup = powerup;

            return pickup;
        }

        /// <summary>
        /// Selects a random powerup with a specified chance of selecting a default weapon powerup.
        /// </summary>
        private Powerup SelectPowerup(float defaultWeaponPowerupOverrideChance)
        {
            Powerup powerup;

            if (AssignableDefaultWeaponPowerups.Any() && RandomUtil.Bool(defaultWeaponPowerupOverrideChance))
                powerup = RandomUtil.RandomElement(AssignableDefaultWeaponPowerups);
            else
                powerup = RandomUtil.RandomElement(AssignableBasicPowerups);

            return powerup;
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
            {
                if (powerup is BasicPowerup basic)
                    AssignableBasicPowerups.Add(basic);
                else if (powerup is DefaultWeaponPowerup defaultWeapon)
                    AssignableDefaultWeaponPowerups.Add(defaultWeapon);
                else
                    throw ExceptionUtil.ArgumentException(() => powerup);
            }
        }
    }
}
