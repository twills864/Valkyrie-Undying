using Assets.ColorManagers;
using Assets.UI;
using UnityEngine;

namespace Assets.ObjectPooling
{
    // Will expand to other UI elements in the future
    /// <inheritdoc/>
    public class UIElementPoolList : PoolList<UIElement>
    {
#pragma warning disable 0414

        [SerializeField]
        private FleetingText FleetingTextPrefab = null;
        [SerializeField]
        private AtomTrail AtomTrailPrefab = null;
        [SerializeField]
        private VictimMarker VictimMarkerPrefab = null;
        [SerializeField]
        private VictimMarkerCorner VictimMarkerCornerPrefab = null;
        [SerializeField]
        private EnemyHealthBar EnemyHealthBarPrefab = null;
        [SerializeField]
        private MetronomeLabel MetronomeLabelPrefab = null;

#pragma warning restore 0414

        protected override Color GetDefaultColor(in ColorManager colorManager)
            => Color.white;

        protected override void OnInitSprites(in ColorManager colorManager)
        {
            Color defaultPlayer = colorManager.DefaultPlayer;
            AtomTrailPrefab.SpriteColor = defaultPlayer;

            float victimMarkerAlpha = colorManager.UI.VictimMarkerAlpha;
            VictimMarkerCornerPrefab.SpriteColor = colorManager.SetAlpha(defaultPlayer, victimMarkerAlpha);
        }

        protected override void OnInit()
        {
            VictimMarker.StaticInit();
            MetronomeLabel.StaticInit();
        }
    }
}
