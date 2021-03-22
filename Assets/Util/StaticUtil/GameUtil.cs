using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;
using Assets.Enemies;
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
                if (gameObject != null && gameObject.TryGetComponent<Enemy>(out enemy))
                {
                    raycastHit = hit;
                    return true;
                }
            }

            enemy = null;
            raycastHit = null;
            return false;
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
