using System;
using Assets.Util;
using UnityEngine;

namespace Assets.ScreenEdgeColliders
{
    public class ScreenEdgeCollider : MonoBehaviour
    {
        private float ColliderWidth = 1.0f;

        public ScreenSide ScreenSide { get; private set; }

        public void Init(ScreenSide side)
        {
            ScreenSide = side;

            bool isWide;
            bool isNegativeOffset;

            Vector2 size;
            Vector2 position;

            switch (side)
            {
                case ScreenSide.Top:
                    isWide = true;
                    isNegativeOffset = false;
                    break;
                case ScreenSide.Right:
                    isWide = false;
                    isNegativeOffset = false;
                    break;
                case ScreenSide.Bottom:
                    isWide = true;
                    isNegativeOffset = true;
                    break;
                case ScreenSide.Left:
                    isWide = false;
                    isNegativeOffset = true;
                    break;
                default:
                    throw ExceptionUtil.ArgumentException(() => side);
            }

            float negativePositionMultiplier = isNegativeOffset ? -1f : 1f;
            float positionOffset = ColliderWidth * -0.5f;

            if(isWide)
            {
                size = new Vector2(SpaceUtil.WorldMapSize.x, ColliderWidth);
                position = new Vector2(0, (SpaceUtil.WorldMap.Top.y - positionOffset) * negativePositionMultiplier);
            }
            else
            {
                size = new Vector2(ColliderWidth, SpaceUtil.WorldMapSize.y);
                position = new Vector2((SpaceUtil.WorldMap.Right.x - positionOffset) * negativePositionMultiplier, 0);
            }

            var collider = GetComponent<BoxCollider2D>();

            collider.size = size;
            collider.transform.position = position;
        }
    }
}