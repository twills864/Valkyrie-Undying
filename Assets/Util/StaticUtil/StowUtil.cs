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
    /// <summary>
    /// A collection of utility methods used to stow active objects off-screen until they're needed.
    /// </summary>
    public static class StowUtil
    {
        public const float LaserEnemyBulletStowX = 1000f;

        public static void StowX(ValkyrieSprite target, float x)
        {
            target.PositionX += x;
        }

        public static void UnstowX(ValkyrieSprite target, float x)
        {
            target.PositionX -= x;
        }
    }
}
