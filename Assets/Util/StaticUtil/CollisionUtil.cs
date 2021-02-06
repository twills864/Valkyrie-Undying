using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Util
{
    public static class CollisionUtil
    {
        public const string TagDestructor = "Destructor";
        public const string TagPlayerBullet = "PlayerBullet";
        public const string TagPlayer = "Player";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDestructor(Collider2D collision)
        {
            return collision.tag == TagDestructor;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPlayerBullet(Collider2D collision)
        {
            return collision.tag == TagPlayerBullet;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPlayer(Collider2D collision)
        {
            return collision.tag == TagPlayer;
        }
    }

}