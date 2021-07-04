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
    /// Contains definitions for the physics layers
    /// specific Unity GameObjects will be located on.
    /// </summary>
    public static class LayerUtil
    {
        public const int SourceLayerEnemyBullets = 8;
        public const int SourceLayerEnemies = 9;
        public const int SourceLayerPlayer = 10;
        public const int SourceLayerPlayerBullets = 11;
        public const int SourceLayerScreenEdge = 12;
        public const int SourceLayerPickups = 13;
        public const int SourceLayerBackground = 14;

        public const int LayerEnemyBullets = 1 << SourceLayerEnemyBullets;
        public const int LayerEnemies = 1 << SourceLayerEnemies;
        public const int LayerPlayer = 1 << SourceLayerPlayer;
        public const int LayerPlayerBullets = 1 << SourceLayerPlayerBullets;
        public const int LayerScreenEdge = 1 << SourceLayerScreenEdge;
        public const int LayerPickups = 1 << SourceLayerPickups;
        public const int LayerBackground = 1 << SourceLayerBackground;

        //public static LayerMask MaskEnemyBullets => 1 << LayerEnemyBullets;
        //public static LayerMask MaskEnemies => 1 << LayerEnemies;
        //public static LayerMask MaskPlayer => 1 << LayerPlayer;
        //public static LayerMask MaskPlayerBullets => 1 << LayerPlayerBullets;
        //public static LayerMask MaskScreenEdge => 1 << LayerScreenEdge;
        //public static LayerMask MaskPickups => 1 << LayerPickups;
        //public static LayerMask MaskBackground => 1 << LayerBackground;
    }
}
