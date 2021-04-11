﻿using System.Collections.Generic;
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

#pragma warning restore 0414

        public List<Powerup> AllAssignablePowerups { get; private set; }

        protected override Color GetDefaultColor(in ColorManager colorManager)
            => Color.white;

        protected override void OnInitSprites(in ColorManager colorManager)
        {
            // TODO: Load powerups, or refactor into somewhere else
        }

        public void InitializePowerups(List<Powerup> allPowerups)
        {
            // Create new List so that we can remove elements or otherwise modify this list as needed.
            AllAssignablePowerups = new List<Powerup>(allPowerups);
        }

        public PowerupPickup GetRandomPowerup(Vector3 position)
        {
            var pickup = Get<PowerupPickup>(position);
            var powerup = RandomUtil.RandomElement(AllAssignablePowerups);
            pickup.TargetPowerup = powerup;

            return pickup;
        }
    }
}