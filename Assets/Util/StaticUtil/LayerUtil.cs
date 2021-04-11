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
    public static class LayerUtil
    {
        public const int LayerEnemyBullets = 1 << 8;
        public const int LayerEnemies = 1 << 9;
        public const int LayerPlayer = 1 << 10;
        public const int LayerPlayerBullets = 1 << 11;
        public const int LayerScreenEdge = 1 << 12;
        public const int LayerPickups = 1 << 13;

        //public static LayerMask MaskEnemyBullets => 1 << LayerEnemyBullets;
        //public static LayerMask MaskEnemies => 1 << LayerEnemies;
        //public static LayerMask MaskPlayer => 1 << LayerPlayer;
        //public static LayerMask MaskPlayerBullets => 1 << LayerPlayerBullets;
        //public static LayerMask MaskScreenEdge => 1 << LayerScreenEdge;
        //public static LayerMask MaskPickups => 1 << LayerPickups;
    }
}
