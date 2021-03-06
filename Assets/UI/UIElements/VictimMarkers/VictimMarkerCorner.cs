using Assets.GameTasks;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.UI
{
    /// <inheritdoc/>
    public class VictimMarkerCorner : UIElement
    {
        [SerializeField]
        private SpriteRenderer Sprite;
        protected override ColorHandler DefaultColorHandler()
            => new SpriteColorHandler(Sprite);

        /// <summary>
        /// The calculated offset from the center of the host VictimMarker.
        /// </summary>
        public Vector3 PositionOffset { get; set; }

        protected override void OnActivate()
        {
            Alpha = 0;
        }
    }
}