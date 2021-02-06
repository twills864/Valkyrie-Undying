using Assets.UI;
using UnityEngine;

namespace Assets.Util.ObjectPooling
{
    // Will expand to other UI elements in the future
    /// <inheritdoc/>
    public class UIElementPoolList : PoolList<FleetingText>
    {
        [SerializeField]
        private FleetingText FleetingTextPrefab;
    }
}
