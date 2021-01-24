using Assets.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    // The object that detects when objects have left the scene, and therefore should be removed
    public class Destructor : MonoBehaviour
    {
        public void Init()
        {
            transform.position = Vector3.zero;

            var worldMapSize = SpaceUtil.WorldMapSize;
            GetComponent<BoxCollider2D>().size = worldMapSize;

            GetComponent<SpriteRenderer>().enabled = false;
        }

    }
}