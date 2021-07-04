using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.Hierarchy.ColorHandlers;
using Assets.Util;
using UnityEngine;

namespace Assets.UI
{
    /// <summary>
    /// A bar that renders a sprite repeating an arbitrary number of times.
    /// If the sprites would go beyond the visible screen, the space
    /// between the sprites decreases to show them all.
    /// </summary>
    /// <inheritdoc/>
    public class RepeatingSpriteBar : UIElement
    {
        #region Prefabs

        [SerializeField]
        private Sprite _Sprite = null;

        [SerializeField]
        private float _ScreenMargin = GameConstants.PrefabNumber;

        [SerializeField]
        private float _SpriteMargin = GameConstants.PrefabNumber;

        #endregion Prefabs


        #region Prefab Properties

        private Sprite Sprite => _Sprite;
        private float ScreenMargin { get; set; }
        private float SpriteMargin { get; set; }

        #endregion Prefab Properties

        public Vector2 SpriteSize => Sprite.bounds.size;

        protected override ColorHandler DefaultColorHandler()
        {
            return new NullColorHandler();
        }

        public int SpritesToRender { get; set; }

        private Vector2 SpriteRenderScreenSize { get; set; }

        private float ScreenY { get; set; }
        private float RenderStartX { get; set; }
        private float MaxRenderX { get; set; }
        private float MaxRenderWidth { get; set; }

        private float SpritesOffsetFromTop { get; set; }

        public void Init(float spritesOffsetFromTop)
        {
            // + 0.5f to account for the fact that position is
            // calculated from the cente of the sprite.
            SpritesOffsetFromTop = spritesOffsetFromTop + 0.5f;
            Init();
        }

        protected sealed override void OnUIElementInit()
        {
            Vector3 world0 = Camera.main.WorldToScreenPoint(Vector3.zero);

            float GetScreenPosition(float worldPosition)
            {
                Vector3 world = Camera.main.WorldToScreenPoint(new Vector3(worldPosition, 0));
                float ret = world.x - world0.x;
                return ret;
            }

            ScreenMargin = GetScreenPosition(_ScreenMargin);
            SpriteMargin = GetScreenPosition(_SpriteMargin);

            Vector3 worldBound = Camera.main.WorldToScreenPoint(SpriteSize);

            float x = worldBound.x - world0.x;
            float y = worldBound.y - world0.y;

            SpriteRenderScreenSize = new Vector2(x, y);


            RenderStartX = ScreenMargin;

            var rightX = GetScreenPosition(SpaceUtil.WorldMap.Width);
            MaxRenderX = rightX - RenderStartX - SpriteRenderScreenSize.x;

            MaxRenderWidth = MaxRenderX - RenderStartX;


            // In GUI coordinates, 0 = top of screen
            float positionY = SpaceUtil.WorldMap.Top.y - (SpriteSize.y * SpritesOffsetFromTop);
            var screenPos = GetScreenPosition(positionY);
            screenPos = SpaceUtil.ScreenMap.Center.y - screenPos - (SpriteRenderScreenSize.y * 0.5f);

            ScreenY = screenPos;
        }

        private void OnGUI()
        {
            float totalWidth = (SpriteRenderScreenSize.x * SpritesToRender);
            totalWidth += SpriteMargin * (SpritesToRender - 1);

            float spaceBetweenSprites;
            if (totalWidth <= MaxRenderWidth)
                spaceBetweenSprites = SpriteMargin + SpriteRenderScreenSize.x;
            else
                spaceBetweenSprites = (MaxRenderWidth) / (SpritesToRender - 1);

            for(int i = SpritesToRender - 1; i >= 0; i--)
            {
                float x = RenderStartX + (i * spaceBetweenSprites);
                DrawSprite(x);
            }
        }

        private void DrawSprite(float xPosition)
        {
            Vector2 position = new Vector2(xPosition, ScreenY);
            Rect rect = new Rect(position, SpriteRenderScreenSize);
            GUI.DrawTexture(rect, Sprite.texture, ScaleMode.ScaleAndCrop);
        }
    }
}
