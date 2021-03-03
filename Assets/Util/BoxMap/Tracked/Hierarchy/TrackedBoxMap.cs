using UnityEngine;

namespace Assets.Util
{
    /// <summary>
    /// Tracks the given MonoBehaviour, and
    /// dynamically calculates BoxMap information as needed.
    /// </summary>
    public abstract class TrackedBoxMap : IBoxMap
    {
        protected MonoBehaviour Target { get; }

        protected abstract Bounds Bounds { get; }
        protected Vector2 Size => Bounds.size;


        public TrackedBoxMap(MonoBehaviour target)
        {
            Target = target;
        }

        private const float NegativeHalf = -0.5f;
        private const float Half = 0.5f;
        private const float Zero = 0f;

        public Vector2 TopLeft => ScaleFromCenter(NegativeHalf, Half);
        public Vector2 Top => ScaleFromCenter(Zero, Half);
        public Vector2 TopRight => ScaleFromCenter(Half, Half);
        public Vector2 Right => ScaleFromCenter(Half, Zero);
        public Vector2 BottomRight => ScaleFromCenter(Half, NegativeHalf);
        public Vector2 Bottom => ScaleFromCenter(Zero, NegativeHalf);
        public Vector2 BottomLeft => ScaleFromCenter(NegativeHalf, NegativeHalf);
        public Vector2 Left => ScaleFromCenter(NegativeHalf, 0);
        public Vector2 Center => Bounds.center;

        public float X => Center.x;
        public float Y => Center.y;
        public float Width => Size.x;
        public float Height => Size.y;

        public float WidthHalf => Width * Half;
        public float HeightHalf => Height * Half;

        private Vector2 ScaleFromCenter(float xScale, float yScale)
        {
            Vector2 size = Size;
            Vector2 add = new Vector2(size.x * xScale, size.y * yScale);
            Vector2 ret = Center + add;

            return ret;
        }
    }
}
