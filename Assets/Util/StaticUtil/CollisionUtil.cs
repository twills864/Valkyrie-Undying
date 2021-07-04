using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Assets.Enemies;
using Assets.ScreenEdgeColliders;
using UnityEngine;

namespace Assets.Util
{
    /// <summary>
    /// Contains useful methods involving Unity physics collisions.
    /// </summary>
    public static class CollisionUtil
    {
        public const string TagDestructor = "Destructor";
        public const string TagPlayerBullet = "PlayerBullet";
        public const string TagPlayer = "Player";
        public const string TagEnemy = "Enemy";
        public const string TagEnemyBullet = "EnemyBullet";
        public const string TagScreenEdge = "ScreenEdge";
        public const string TagPickup = "Pickup";

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEnemy(Collider2D collision)
        {
            return collision.tag == TagEnemy;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEnemyBullet(Collider2D collision)
        {
            return collision.tag == TagEnemyBullet;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsScreenEdge(Collider2D collision)
        {
            return collision.tag == TagScreenEdge;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsScreenEdge(Collider2D collision, out ScreenSide screenSide)
        {
            bool ret = IsScreenEdge(collision);
            if(ret)
            {
                var collider = collision.GetComponent<ScreenEdgeCollider>();
                screenSide = collider.ScreenSide;
            }
            else
            {
                // Default
                screenSide = ScreenSide.Bottom;
            }
            return ret;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPickup(Collider2D collision)
        {
            return collision.tag == TagPickup;
        }


        /// <summary>
        /// Returns each enemy that is currently colliding with a specified <paramref name="collider"/>.
        /// </summary>
        /// <param name="collider">The Collider to check.</param>
        /// <returns>Each next enemy that is currently colliding with the specified collider.</returns>
        public static IEnumerable<Enemy> GetAllEnemiesCollidingWith(Collider2D collider)
        {
            ContactFilter2D enemyFilter = new ContactFilter2D()
            {
                useTriggers = true,
                layerMask = Physics2D.GetLayerCollisionMask(LayerUtil.SourceLayerEnemies),
                useLayerMask = true,
            };

            List<Collider2D> collisions = new List<Collider2D>();
            collider.OverlapCollider(enemyFilter, collisions);

            foreach(var collision in collisions)
            {
                if(collision.TryGetComponent<Enemy>(out Enemy enemy))
                    yield return enemy;
            }
        }
    }

}