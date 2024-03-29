﻿using UnityEngine;

namespace Assets.Util
{
    /// <summary>
    /// Precalculates BoxMap information based on constant information.
    /// </summary>
    /// <inheritdoc />
    public class BoxMap : IBoxMap
    {
        public Vector3 TopLeft { get; }
        public Vector3 Top { get; }
        public Vector3 TopRight { get; }
        public Vector3 Right { get; }
        public Vector3 BottomRight { get; }
        public Vector3 Bottom { get; }
        public Vector3 BottomLeft { get; }
        public Vector3 Left { get; }
        public Vector3 Center { get; }

        public float X { get; }
        public float Y { get; }
        public float Width { get; }
        public float Height { get; }

        public float WidthHalf { get; }
        public float HeightHalf { get; }

        private Rect Rect { get; }

        public BoxMap(Rect rect) : this(rect.x, rect.y, rect.width, rect.height)
        {

        }
        public BoxMap(Vector3 position, Vector2 size) : this(position.x, position.y, size.x, size.y)
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

            TopLeft = new Vector3(X, Y + Height);
            Top = new Vector3(X + WidthHalf, Y + Height);
            TopRight = new Vector3(X + Width, Y + Height);
            Right = new Vector3(X + Width, Y + HeightHalf);
            BottomRight = new Vector3(X + Width, Y);
            Bottom = new Vector3(X + WidthHalf, Y);
            BottomLeft = new Vector3(X, Y);
            Left = new Vector3(X, Y + HeightHalf);
            Center = new Vector3(X + WidthHalf, Y + HeightHalf);

            Rect = new Rect(X, Y, Width, Height);
        }

        public bool ContainsPoint(Vector3 point)
        {
            bool ret = Rect.Contains(point);
            return ret;
        }

        public bool ContainsXCoordinate(float x)
        {
            bool ret = x >= Left.x
                && x <= Right.x;
            return ret;
        }

        public bool ContainsYCoordinate(float y)
        {
            bool ret = y >= Bottom.y
                && y <= Top.y;
            return ret;
        }

        public float RatioOfWidth(float x)
        {
            float xDiff = x - Left.x;
            float ratio = xDiff / Width;

            return ratio;
        }

        public float ClampedRatioOfWidth(float x)
        {
            float ratio = RatioOfWidth(x);
            float ret = Mathf.Clamp(ratio, 0f, 1f);

            return ret;
        }
    }
}
