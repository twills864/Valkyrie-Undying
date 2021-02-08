using Assets.Util;
using UnityEngine;

namespace Assets
{
    // The object that detects when objects have left the scene, and therefore should be removed
    /// <inheritdoc/>
    public class Destructor : MonoBehaviour
    {
        private const float Buffer = 2.0f;
        private Vector2 BufferVector => new Vector2(Buffer, Buffer);
        public void Init()
        {
            transform.position = Vector3.zero;

            var worldMapSize = SpaceUtil.WorldMapSize + BufferVector;
            GetComponent<BoxCollider2D>().size = worldMapSize;

            GetComponent<SpriteRenderer>().enabled = false;
        }

    }
}