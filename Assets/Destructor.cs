﻿using Assets.Util;
using UnityEngine;

namespace Assets
{
    /// <summary>
    /// The invisible object that encompasses the entire visible screen
    /// plus a small buffer, and detects when objects have left the scene.
    /// These leaving objects should usually be deactivated or otherwise
    /// removed from the scene.
    /// </summary>
    /// <inheritdoc/>
    public class Destructor : MonoBehaviour
    {
        public const float Buffer = 4.0f;
        private Vector2 BufferVector => new Vector2(Buffer, Buffer);

        private Vector2 _size;
        public Vector2 Size {
            get => _size;
            private set
            {
                _size = value;
                SizeHalf = _size * 0.5f;

            }
        }
        public Vector2 SizeHalf { get; private set; }
        public void Init()
        {
            transform.position = new Vector3(0, 0, SpaceUtil.DeepZPosition);

            var worldMapSize = SpaceUtil.WorldMapSize + BufferVector;
            Size = worldMapSize;

            GetComponent<BoxCollider2D>().size = worldMapSize;
            GetComponent<SpriteRenderer>().enabled = false;

            SpaceUtil.InitDestructorMap(this);
        }


        private void OnMouseDown()
        {
            GameManager.Instance.CreateFleetingText("DESTRUCTOR MOUSE DOWN", SpaceUtil.WorldMap.Center);
        }
    }
}