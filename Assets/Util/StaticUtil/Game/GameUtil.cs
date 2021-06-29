using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.Enemies;
using Assets.Hierarchy;
using UnityEngine;

namespace Assets.Util
{
    public static class GameUtil
    {
        /// <summary>
        /// Gives an extra level to a given weapon level if it's equal to the
        /// maximum obtainable weapon level in the game.
        /// Generally, weapons gain an additional boost for achieving the maximum level.
        /// </summary>
        /// <param name="weaponLevel">The current weapon level.</param>
        /// <returns>The current weapon level plus one
        /// if it's the current level is the maximum obtainable weapon level.
        /// Otherwise, returns the current weapon level.</returns>
        public static int PlusOneIfMaxWeaponLevel(int weaponLevel)
        {
            int ret = weaponLevel != GameConstants.MaxWeaponLevel ? weaponLevel : (weaponLevel + 1);
            return ret;
        }

        /// <summary>
        /// Attempts to get the first enemy from a given start <paramref name="position"/>
        /// in the direction of a given <paramref name="direction"/>
        /// </summary>
        /// <param name="position">The start position.</param>
        /// <param name="direction">The direction to check.</param>
        /// <param name="enemy">The first enemy hit.</param>
        /// <param name="raycastHit">The RaycastHid2D object of the represented hit.</param>
        /// <returns>True if an enemy is hit; False otherwise.</returns>
        public static bool RaycastTryGetEnemy(Vector2 position, Vector2 direction, out Enemy enemy, out RaycastHit2D? raycastHit)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(position, direction);

            foreach (var hit in hits)
            {
                var gameObject = hit.collider?.gameObject;
                if (gameObject != null)
                {
                    if (gameObject.TryGetComponent<Enemy>(out enemy))
                    {
                        raycastHit = hit;
                        return true;
                    }
                    else if(gameObject.TryGetComponent<IRaycastTrigger>(out IRaycastTrigger trigger))
                        trigger.ActivateTrigger(hit.point);
                }
            }

            enemy = null;
            raycastHit = null;
            return false;
        }


        /// <summary>
        /// Attempts to get all enemies in between a given start position
        /// and a given end position.
        /// </summary>
        /// <param name="startPosition">The start position.</param>
        /// <param name="endPosition">The end position.</param>
        /// <returns>Each next enemy hit.</returns>
        public static IEnumerable<EnemyRaycastHit> LinecastGetAllEnemies(Vector2 startPosition, Vector2 endPosition)
        {

#if UNITY_EDITOR
            if (startPosition == endPosition)
            {
                Debug.Break();
                System.Diagnostics.Debugger.Break();
            }
#endif

            RaycastHit2D[] hits = Physics2D.LinecastAll(startPosition, endPosition, LayerUtil.LayerEnemies);

            foreach (var hit in hits)
            {
                var gameObject = hit.collider?.gameObject;

                if (gameObject.TryGetComponent<Enemy>(out var enemy))
                {
                    var ret = new EnemyRaycastHit(enemy, hit);
                    yield return ret;
                }
                else if (gameObject.TryGetComponent<IRaycastTrigger>(out IRaycastTrigger trigger))
                    trigger.ActivateTrigger(hit.point);
            }
        }

        /// <summary>
        /// Gets all enemies and IRaycastTriggers in between a given start position
        /// and a given end position.
        ///
        /// Accepts an existing array to place results into as an optimization,
        /// in case this method needs to be called frequently.
        /// </summary>
        /// <param name="startPosition">The start position.</param>
        /// <param name="endPosition">The end position.</param>
        /// <param name="nonallocArray">The array to place results into.</param>
        /// <returns>The number of enemies hit.</returns>
        public static int LinecastGetAllEnemiesAndRaycastTriggersNonAlloc(Vector2 startPosition, Vector2 endPosition, RaycastHit2D[] nonallocArray)
        {

#if UNITY_EDITOR
            if (startPosition == endPosition)
            {
                Debug.Break();
                System.Diagnostics.Debugger.Break();
            }
#endif

            int numHits = Physics2D.LinecastNonAlloc(startPosition, endPosition, nonallocArray, LayerUtil.LayerEnemies);

            return numHits;
        }

        /// <summary>
        /// Confirms whether or not the player is hit by a raycast from a given start <paramref name="position"/>
        /// in a given <paramref name="direction"/>.
        /// </summary>
        /// <param name="position">The start position.</param>
        /// <param name="direction">The direction to check.</param>
        /// <param name="raycastHit">The RaycastHid2D object of the represented hit.</param>
        /// <returns>True if the player is hit; False otherwise.</returns>
        /// <returns></returns>
        public static bool RaycastHitsPlayer(Vector2 position, Vector2 direction, out RaycastHit2D raycastHit)
        {
            raycastHit = Physics2D.Raycast(position, direction, float.PositiveInfinity, LayerUtil.LayerPlayer);
            bool ret = raycastHit.collider != null;
            return ret;
        }
    }
}
