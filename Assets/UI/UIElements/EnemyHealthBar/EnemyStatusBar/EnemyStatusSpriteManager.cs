using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Hierarchy.ColorHandlers;
using UnityEngine;

namespace Assets.UI.UIElements.EnemyHealthBar
{
    /// <summary>
    /// Sorts and displays all status effects currently experienced by an enemy.
    /// </summary>
    public class EnemyStatusSpriteManager
    {
        private const float WorkingSpriteHeight = 0.35f;
        private const float VerticalMargin = 0.15f;

        private List<EnemyStatusSprite> Sprites { get; set; }
        private List<EnemyStatusSprite> ActiveSprites { get; set; }

        public Vector3 Position { get; set; }



        public EnemyStatusSpriteManager(List<EnemyStatusSprite> sprites)
        {
            Sprites = sprites;
            ActiveSprites = new List<EnemyStatusSprite>(sprites.Count);

            Position = Vector3.zero;

            foreach (var sprite in Sprites)
                sprite.Init();
        }

        public void Init()
        {
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


            float totalHeight = (ActiveSprites.Count - 1) * WorkingSpriteHeight;
            float totalMargin = (ActiveSprites.Count - 1) * VerticalMargin;

            float offsetY = (totalHeight + totalMargin) * 0.5f;

            foreach(var sprite in ActiveSprites)
            {
                sprite.LocalPositionY = offsetY; // + (sprite.Height * 0.5f);
                offsetY -= (WorkingSpriteHeight + VerticalMargin);
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
