using Assets.GameTasks;
using Assets.Util;
using UnityEngine;

namespace Assets.UI
{
    /// <inheritdoc/>
    public class VictimMarkerCorner : UIElement
    {
        [SerializeField]
        private SpriteRenderer Sprite;

        /// <summary>
        /// The calculated offset from the center of the host VictimMarker.
        /// </summary>
        public Vector3 PositionOffset { get; set; }

        public float Alpha
        {
            get => Sprite.color.a;
            set
            {
                var color = Sprite.color;
                color.a = value;
                Sprite.color = color;
            }
        }

        protected override void OnActivate()
        {
            Alpha = 0;
        }
    }
}