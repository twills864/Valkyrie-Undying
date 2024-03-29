﻿using UnityEngine;

namespace Assets.Util
{
    /// <summary>
    /// Tracks the given MonoBehaviour, and
    /// dynamically calculates BoxMap information as needed.
    /// </summary>
    /// <inheritdoc/>
    public abstract class TrackedBoxMap : IBoxMap
    {
        protected MonoBehaviour Target { get; }

        protected abstract Bounds Bounds { get; }
        public Vector3 Size => Bounds.size;


        public TrackedBoxMap(MonoBehaviour target)
        {
            Target = target;
        }

        private const float NegativeHalf = -0.5f;
        private const float Half = 0.5f;
        private const float Zero = 0f;

        public Vector3 TopLeft => ScaleFromCenter(NegativeHalf, Half);
        public Vector3 Top => ScaleFromCenter(Zero, Half);
        public Vector3 TopRight => ScaleFromCenter(Half, Half);
        public Vector3 Right => ScaleFromCenter(Half, Zero);
        public Vector3 BottomRight => ScaleFromCenter(Half, NegativeHalf);
        public Vector3 Bottom => ScaleFromCenter(Zero, NegativeHalf);
        public Vector3 BottomLeft => ScaleFromCenter(NegativeHalf, NegativeHalf);
        public Vector3 Left => ScaleFromCenter(NegativeHalf, 0);
        public Vector3 Center => Bounds.center;

        public float X => Center.x;
        public float Y => Center.y;
        public float Width => Size.x;
        public float Height => Size.y;

        public float WidthHalf => Width * Half;
        public float HeightHalf => Height * Half;

        private Vector3 ScaleFromCenter(float xScale, float yScale)
        {
            Vector3 size = Size;
            Vector3 add = new Vector2(size.x * xScale, size.y * yScale);
            Vector3 ret = Center + add;

            return ret;
        }

        public bool ContainsPoint(Vector3 point)
        {
            Rect rect = new Rect(X, Y, Width, Height);
            bool ret = rect.Contains(point);
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
