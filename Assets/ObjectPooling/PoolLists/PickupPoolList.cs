using System;
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
        private PiercingRoundsPowerup PiercingRoundsPowerup { get; set; }

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

            PiercingRoundsPowerup = AssignableDefaultWeaponPowerups
                .Where(x => x.GetType() == typeof(PiercingRoundsPowerup))
                .First() as PiercingRoundsPowerup;

            AssignableDefaultWeaponPowerups.Remove(PiercingRoundsPowerup);
        }

        public PowerupPickup GetRandomBasicPowerupPickup(Vector3 position)
        {
            var powerup = RandomUtil.RandomElement(AssignableBasicPowerups);
            return FinishSpawningPowerup(powerup, position);
        }

        public PowerupPickup GetRandomDefaultWeaponPowerupPickup(Vector3 position)
        {
            var powerup = RandomUtil.RandomElement(AssignableDefaultWeaponPowerups);
            return FinishSpawningPowerup(powerup, position);
        }

        public PowerupPickup GetPiercingRoundsPickup(Vector3 position)
        {
            return FinishSpawningPowerup(PiercingRoundsPowerup, position);
        }

        private PowerupPickup FinishSpawningPowerup(Powerup powerup, Vector3 position)
        {
            CheckPowerupOut(powerup);

            var pickup = Get<PowerupPickup>(position);
            pickup.TargetPowerup = powerup;

            return pickup;
        }




        /// <summary>
        /// Gets a PowerupPickup containing a powerup with a specified type.
        /// </summary>
        /// <typeparam name="TPowerup">The type of PowerupPickup to get.</typeparam>
        /// <param name="position">The world position to spawn the PowerupPickup at.</param>
        /// <returns>The retrieved PowerupPickup.</returns>
        public PowerupPickup GetSpecificPowerup<TPowerup>(Vector3 position) where TPowerup : Powerup
        {
            return GetSpecificPowerup(position, typeof(TPowerup));
        }

        /// <summary>
        /// Gets a PowerupPickup containing a powerup with a specified type.
        /// </summary>
        /// <param name="powerupType">The type of PowerupPickup to get.</param>
        /// <param name="position">The world position to spawn the PowerupPickup at.</param>
        /// <returns>The retrieved PowerupPickup.</returns>
        public PowerupPickup GetSpecificPowerup(Vector3 position, Type powerupType)
        {
            Powerup powerup;

            if (typeof(BasicPowerup).IsAssignableFrom(powerupType))
                powerup = AssignableBasicPowerups.Find(m => m.GetType() == powerupType);
            else
                powerup = AssignableDefaultWeaponPowerups.Find(m => m.GetType() == powerupType);

            return GetSpecificPowerup(position, powerup);
        }

        /// <summary>
        /// Gets a PowerupPickup containing the specified <paramref name="powerup"/>.
        /// </summary>
        /// <param name="powerup">The powerup to put in the PowerupPickup.</param>
        /// <param name="position">The world position to spawn the PowerupPickup at.</param>
        /// <returns>The retrieved PowerupPickup.</returns>
        public PowerupPickup GetSpecificPowerup(Vector3 position, Powerup powerup)
        {
            if (powerup != null)
            {
                CheckPowerupOut(powerup);

                var pickup = Get<PowerupPickup>(position);
                pickup.TargetPowerup = powerup;

                return pickup;
            }
            else
                return null;
        }

        /// <summary>
        /// Checks a given <paramref name="powerup"/> out, and
        /// removes it from the appropriate assignable powerup list if necessary.
        /// </summary>
        /// <param name="powerup">The powerup to check out.</param>
        private void CheckPowerupOut(Powerup powerup)
        {
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
