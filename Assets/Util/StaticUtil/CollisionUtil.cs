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
        public const string TagDestructor = "Destructor";
        public const string TagPlayerBullet = "PlayerBullet";
        public const string TagPlayer = "Player";

        public static bool IsDestructor(Collider2D collision)
        {
            return collision.tag == TagDestructor;
        }

        public static bool IsPlayerBullet(Collider2D collision)
        {
            return collision.tag == TagPlayerBullet;
        }

        public static bool IsPlayer(Collider2D collision)
        {
            return collision.tag == TagPlayer;
        }
    }

}