using Assets.ColorManagers;
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
        private AtomTrail AtomTrailPrefab;
        [SerializeField]
        private VictimMarker VictimMarkerPrefab;
        [SerializeField]
        private VictimMarkerCorner VictimMarkerCornerPrefab;
        [SerializeField]
        private EnemyHealthBar EnemyHealthBarPrefab;

        protected override Color GetDefaultColor(ColorManager colorManager)
            => Color.white;

        protected override void OnInitSprites(ColorManager colorManager)
        {
            Color defaultPlayer = colorManager.DefaultPlayer;
            AtomTrailPrefab.SpriteColor = defaultPlayer;

            float victimMarkerAlpha = colorManager.UI.VictimMarkerAlpha;
            VictimMarkerCornerPrefab.SpriteColor = colorManager.SetAlpha(defaultPlayer, victimMarkerAlpha);
        }

        protected override void OnInit()
        {
            VictimMarker.StaticInit();
        }
    }
}
