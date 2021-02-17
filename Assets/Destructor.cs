using Assets.Util;
using UnityEngine;

namespace Assets
{
    // The object that detects when objects have left the scene, and therefore should be removed
    /// <inheritdoc/>
    public class Destructor : MonoBehaviour
    {
        public const float Buffer = 2.0f;
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
            transform.position = Vector3.zero;

            var worldMapSize = SpaceUtil.WorldMapSize + BufferVector;
            Size = worldMapSize;

            GetComponent<BoxCollider2D>().size = worldMapSize;
            GetComponent<SpriteRenderer>().enabled = false;
        }

    }
}