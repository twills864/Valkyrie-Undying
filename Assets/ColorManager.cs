using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.ColorManagers.SubManagers;
using UnityEngine;

namespace Assets.ColorManagers
{
    /// <summary>
    /// A collection of colors designed to be edited in the GameManager Unity object.
    /// </summary>
    [Serializable]
    public struct ColorManager
    {
        public Color DefaultPlayer;
        public Color DefaultEnemy;

        [SerializeField]
        public PlayerColors Player;

        [SerializeField]
        public EnemyColors Enemy;

        [SerializeField]
        public UIColors UI;

        public Color SetAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        public Color DefaultPlayerAdditionalColor()
        {
            return SetAlpha(DefaultPlayer, Player.DefaultAdditionalRatio);
        }
    }
}


namespace Assets.ColorManagers.SubManagers
{
    [Serializable]
    public struct PlayerColors
    {
        public float DefaultAdditionalRatio;

        public Color Reflected;
        public Color Retribution;
        public Color Sentinel;
        public Color Void;
    }

    [Serializable]
    public struct EnemyColors
    {
        public Color Tank;
        public Color RingEnemyRing;
    }

    [Serializable]
    public struct UIColors
    {
        public float VictimMarkerAlpha;
    }
}