using Assets.Util.AssetsDebug;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Util
{
    public static class CollisionUtil
    {
        public static bool IsDestructor(Collider2D collision)
        {
            return collision.tag == "Destructor";
        }

        public static bool IsPlayerBullet(Collider2D collision)
        {
            return collision.tag == "PlayerBullet";
        }
    }

}