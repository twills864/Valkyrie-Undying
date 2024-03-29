﻿using UnityEngine;

namespace Assets.Util
{
    /// <summary>
    /// Contains size and location information about a given rectangular area
    /// </summary>
    public interface IBoxMap
    {
        Vector3 TopLeft { get; }
        Vector3 Top { get; }
        Vector3 TopRight { get; }
        Vector3 Right { get; }
        Vector3 BottomRight { get; }
        Vector3 Bottom { get; }
        Vector3 BottomLeft { get; }
        Vector3 Left { get; }
        Vector3 Center { get; }

        float X { get; }
        float Y { get; }
        float Width { get; }
        float Height { get; }

        float WidthHalf { get; }
        float HeightHalf { get; }

        bool ContainsPoint(Vector3 point);

        float RatioOfWidth(float x);
        float ClampedRatioOfWidth(float x);
    }
}
