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

        //protected sealed override void OnUIElementInit()
        //{
        //    Sprite = GetComponent<SpriteRenderer>();
        //}

        protected override void OnActivate()
        {
            Alpha = 0;
        }
    }
}