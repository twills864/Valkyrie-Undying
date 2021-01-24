using UnityEngine;

namespace Assets.Util
{
    // Precalculates BoxMap information based on constant information.
    public class BoxMap : IBoxMap
    {
        public Vector2 TopLeft { get; }
        public Vector2 Top { get; }
        public Vector2 TopRight { get; }
        public Vector2 Right { get; }
        public Vector2 BottomRight { get; }
        public Vector2 Bottom { get; }
        public Vector2 BottomLeft { get; }
        public Vector2 Left { get; }
        public Vector2 Center { get; }

        public float X { get; }
        public float Y { get; }
        public float Width { get; }
        public float Height { get; }

        public float WidthHalf { get; }
        public float HeightHalf { get; }

        public BoxMap(Rect rect) : this(rect.x, rect.y, rect.width, rect.height)
        {
        }
        public BoxMap(Vector2 position, Vector2 size) : this(position.x, position.y, size.x, size.y)
        {
        }
        public BoxMap(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;

            WidthHalf = Width * 0.5f;
            HeightHalf = Height * 0.5f;

            TopLeft = new Vector2(X, Y + Height);
            Top = new Vector2(X + WidthHalf, Y + Height);
            TopRight = new Vector2(X + Width, Y + Height);
            Right = new Vector2(X + Width, Y + HeightHalf);
            BottomRight = new Vector2(X + Width, Y);
            Bottom = new Vector2(X + WidthHalf, Y);
            BottomLeft = new Vector2(X, Y);
            Left = new Vector2(X, Y + HeightHalf);
            Center = new Vector2(X + WidthHalf, Y + HeightHalf);
        }
    }
}
