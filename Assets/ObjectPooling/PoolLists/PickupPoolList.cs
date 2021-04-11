using Assets.ColorManagers;
using Assets.Pickups;
using Assets.UI;
using UnityEngine;

namespace Assets.ObjectPooling
{
    /// <inheritdoc/>
    public class PickupPoolList : PoolList<Pickup>
    {
#pragma warning disable 0414

        [SerializeField]
        private WeaponPickup WeaponPrefab = null;

#pragma warning restore 0414

        protected override Color GetDefaultColor(in ColorManager colorManager)
            => Color.white;

        protected override void OnInitSprites(in ColorManager colorManager)
        {
            // TODO: Load powerups, or refactor into somewhere else
        }
    }
}
