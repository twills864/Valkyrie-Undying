using Assets.UI;
using UnityEngine;

namespace Assets.Util.ObjectPooling
{
    // Will expand to other UI elements in the future
    /// <inheritdoc/>
    public class UIElementPoolList : PoolList<UIElement>
    {
        [SerializeField]
        private FleetingText FleetingTextPrefab;
        [SerializeField]
        private AtomTrail AtomTrialPrefab;
    }
}
