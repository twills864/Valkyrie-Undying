using Assets.UI;
using UnityEngine;

namespace Assets.ObjectPooling
{
    // Will expand to other UI elements in the future
    /// <inheritdoc/>
    public class UIElementPoolList : PoolList<UIElement>
    {
        [SerializeField]
        private FleetingText FleetingTextPrefab;
        [SerializeField]
        private AtomTrail AtomTrialPrefab;
        [SerializeField]
        private VictimMarker VictimMarkerPrefab;
        [SerializeField]
        private VictimMarkerCorner VictimMarkerCornerPrefab;
        [SerializeField]
        private EnemyHealthBar EnemyHealthBarPrefab;
    }
}
