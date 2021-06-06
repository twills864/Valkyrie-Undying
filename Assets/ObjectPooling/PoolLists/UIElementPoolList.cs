using Assets.ColorManagers;
using Assets.UI;
using Assets.UI.UIElements.EnemyHealthBar;
using UnityEngine;

namespace Assets.ObjectPooling
{
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
        //[SerializeField]
        //private EnemyStatusBarHolder EnemyStatusBarHolderPrefab = null;
        //[SerializeField]
        //private EnemyStatusBar EnemyStatusBarPrefab = null;
        //[SerializeField]
        //private EnemyStatusSprite EnemyStatusSpritePrefab = null;
        [SerializeField]
        private MetronomeLabel MetronomeLabelPrefab = null;
        [SerializeField]
        private MortarGuide MortarGuidePrefab = null;
        [SerializeField]
        private LineRendererHost LineRendererHostPrefab = null;
        [SerializeField]
        private ProgressBar ProgressBarPrefab = null;

#pragma warning restore 0414

        protected override Color GetDefaultColor(in ColorManager colorManager)
            => Color.white;

        protected override void OnInitSprites(in ColorManager colorManager)
        {
            Color defaultPlayer = colorManager.DefaultPlayer;
            AtomTrailPrefab.SpriteColor = defaultPlayer;

            float victimMarkerAlpha = colorManager.UI.VictimMarkerAlpha;
            VictimMarkerCornerPrefab.SpriteColor = colorManager.SetAlpha(defaultPlayer, victimMarkerAlpha);

            MortarGuide.StaticInit(in colorManager);
        }

        protected override void OnInit()
        {
            VictimMarker.StaticInit();
            MetronomeLabel.StaticInit();
        }
    }
}
