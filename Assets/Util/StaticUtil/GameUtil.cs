using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Constants;

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
    }
}
