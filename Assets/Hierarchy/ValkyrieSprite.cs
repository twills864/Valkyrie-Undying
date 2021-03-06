using Assets.Hierarchy.ColorHandlers;
using LogUtilAssets;
using UnityEngine;

namespace Assets
{
    public abstract class ValkyrieSprite : Loggable
    {
        protected ColorHandler ColorHandler { get; private set; }
        protected abstract ColorHandler DefaultColorHandler();

        protected virtual void OnInit() { }
        public void Init()
        {
            ColorHandler = DefaultColorHandler();
            OnInit();
        }
        public void Init(Vector2 position)
        {
            transform.position = position;
            Init();
        }

        public Color SpriteColor
        {
            get => ColorHandler.Color;
            set => ColorHandler.Color = value;
        }

        public float Alpha
        {
            get => ColorHandler.Alpha;
            set => ColorHandler.Alpha = value;
        }


        public void RotateSprite(float rotation)
        {
            transform.Rotate(0, 0, rotation);
        }
    }
}
