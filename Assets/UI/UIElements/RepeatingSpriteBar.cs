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

        protected sealed override void OnUIElementInit()
        {
            Vector3 world0 = Camera.main.WorldToScreenPoint(Vector3.zero);
            Vector3 world02 = world0;

            float GetScreenPosition(float worldPosition)
            {
                Vector3 world = Camera.main.WorldToScreenPoint(new Vector3(worldPosition, 0));
                float ret = world.x - world0.x;
                return ret;
            }

            ScreenMargin = GetScreenPosition(_ScreenMargin);
            SpriteMargin = GetScreenPosition(_SpriteMargin);

            var bounds = Sprite.bounds;
            Vector3 worldBound = Camera.main.WorldToScreenPoint(bounds.size);

            float x = worldBound.x - world0.x;
            float y = worldBound.y - world0.y;

            SpriteRenderScreenSize = new Vector2(x, y);


            RenderStartX = ScreenMargin;

            var rightX = GetScreenPosition(SpaceUtil.WorldMap.Width);
            MaxRenderX = rightX - RenderStartX - SpriteRenderScreenSize.x;

            MaxRenderWidth = MaxRenderX - RenderStartX;


            // In GUI coordinates, 0 = top of screen
            var screenPos = GetScreenPosition(transform.position.y);
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
