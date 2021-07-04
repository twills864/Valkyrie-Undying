using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.Pickups
{
    /// <summary>
    /// Renders a sprite in the middle of a pickup.
    /// </summary>
    /// <inheritdoc/>
    public class PickupSpriteHolder : ValkyrieSprite
    {
        /// <summary>
        /// An acceptable size to fit inside of the hexagonal powerup holder.
        /// </summary>
        private const float SizeScale = 0.6f;

        public override TimeScaleType TimeScale => TimeScaleType.Default;
        protected override ColorHandler DefaultColorHandler() => new SpriteColorHandler(SpriteRenderer);

        #region Prefabs

        [SerializeField]
        private SpriteRenderer _SpriteRenderer = null;

        #endregion Prefabs


        #region Prefab Properties

        private SpriteRenderer SpriteRenderer => _SpriteRenderer;

        #endregion Prefab Properties

        public Sprite Sprite
        {
            get => SpriteRenderer.sprite;
            set
            {
                SpriteRenderer.sprite = value;
                RecalculateSpriteScale();
            }
        }

        private Pickup Parent { get; set; }


        public void Init(Pickup pickup, Sprite pickupSprite)
        {
            Parent = pickup;
            Sprite = pickupSprite;

            Init();
        }

        protected override void OnInit()
        {
            RecalculateSpriteScale();
            SpriteColor = Parent.SpriteColor;
        }

        private void RecalculateSpriteScale()
        {
            var thisMap = new SpriteBoxMap(this);

#if UNITY_EDITOR
            Debug.Assert(thisMap.Width == thisMap.Width);
#endif

            float currentWidth = thisMap.Width;

            var parentMap = new SpriteBoxMap(Parent);
            float parentWidth = parentMap.Width;


            float scaleRatio = parentWidth / currentWidth;
            scaleRatio *= SizeScale;

            LocalScale *= scaleRatio;
        }
    }
}
