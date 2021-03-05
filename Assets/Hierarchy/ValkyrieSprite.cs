using LogUtilAssets;
using UnityEngine;

namespace Assets
{
    public class ValkyrieSprite : Loggable
    {
        #pragma warning disable 0649

        [SerializeField]
        private SpriteRenderer PrefabSprite;

        #pragma warning restore 0649

        public SpriteRenderer Sprite
        {
            get => PrefabSprite;
        }


        public Color SpriteColor
        {
            get => Sprite.color;
            set => Sprite.color = value;
        }

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

        public void RotateSprite(float rotation)
        {
            transform.Rotate(0, 0, rotation);
        }
    }
}
