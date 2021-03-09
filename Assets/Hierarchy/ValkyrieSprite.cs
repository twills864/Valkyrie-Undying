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

        #region Transform Position

        public float PositionX
        {
            get => transform.position.x;
            set
            {
                transform.position = new Vector3(value, transform.position.y, transform.position.z);
            }
        }

        public float PositionY
        {
            get => transform.position.y;
            set
            {
                transform.position = new Vector3(transform.position.x, value, transform.position.z);
            }
        }

        public float PositionZ
        {
            get => transform.position.z;
            set
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, value);
            }
        }

        #endregion Transform Position

        #region Transform LocalScale

        public float LocalScaleX
        {
            get => transform.localScale.x;
            set
            {
                transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);
            }
        }

        public float LocalScaleY
        {
            get => transform.localScale.y;
            set
            {
                transform.localScale = new Vector3(transform.localScale.x, value, transform.localScale.z);
            }
        }

        public float LocalScaleZ
        {
            get => transform.localScale.z;
            set
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, value);
            }
        }

        #endregion Transform LocalScale


        public void RotateSprite(float rotation)
        {
            transform.Rotate(0, 0, rotation);
        }
    }
}
