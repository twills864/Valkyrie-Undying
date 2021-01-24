using UnityEngine;

namespace Assets.Util
{
    // Easily allows you to get various information and locations about a given box area
    public interface IBoxMap
    {
        Vector2 TopLeft { get; }
        Vector2 Top { get; }
        Vector2 TopRight { get; }
        Vector2 Right { get; }
        Vector2 BottomRight { get; }
        Vector2 Bottom { get; }
        Vector2 BottomLeft { get; }
        Vector2 Left { get; }
        Vector2 Center { get; }

        float X { get; }
        float Y { get; }
        float Width { get; }
        float Height { get; }

        float WidthHalf { get; }
        float HeightHalf { get; }
    }
}
