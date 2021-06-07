using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Hierarchy.ColorHandlers;
using UnityEngine;

namespace Assets.UI.UIElements.EnemyHealthBar
{
    public class EnemyStatusSpriteManager
    {
        private List<EnemyStatusSprite> Sprites { get; set; }
        private List<EnemyStatusSprite> ActiveSprites { get; set; }

        public Vector3 Position { get; set; }

        private float VerticalMargin { get; set; }

        public EnemyStatusSpriteManager(List<EnemyStatusSprite> sprites)
        {
            Sprites = sprites;
            ActiveSprites = new List<EnemyStatusSprite>(sprites.Count);

            VerticalMargin = sprites.First().VerticalMargin;
            Position = Vector3.zero;

            foreach (var sprite in Sprites)
                sprite.Init();
        }

        public void OnSpawn()
        {
            ActiveSprites.Clear();

            foreach (var sprite in Sprites)
                sprite.OnSpawn();
        }

        public void RecalculateStatusBar()
        {
            RecalculateActiveSprites();

            float totalHeight = (ActiveSprites.Count - 1) * ActiveSprites[0].Height;
            float totalMargin = (ActiveSprites.Count - 1) * VerticalMargin;

            float offsetY = (totalHeight + totalMargin) * 0.5f;

            foreach(var sprite in ActiveSprites)
            {
                sprite.LocalPositionY = offsetY; // + (sprite.Height * 0.5f);
                offsetY -= (sprite.Height + VerticalMargin);
            }
        }

        private void RecalculateActiveSprites()
        {
            ActiveSprites.Clear();

            for (int i = 0; i < Sprites.Count; i++)
            {
                EnemyStatusSprite sprite = Sprites[i];

                if (sprite.IsActive && !ActiveSprites.Contains(sprite))
                    ActiveSprites.Add(sprite);
            }
        }
    }
}
