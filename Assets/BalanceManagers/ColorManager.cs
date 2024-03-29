﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.ColorManagers.SubManagers;
using Assets.Util;
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
        public Color DefaultEnemyAlt;

        [SerializeField]
        public PlayerColors Player;

        [SerializeField]
        public EnemyColors Enemy;

        [SerializeField]
        public UIColors UI;

        [SerializeField]
        public PickupColors Pickup;

        public Color SetPseudoAlpha(Color color, float alpha)
        {
            Color pseudo = Color.Lerp(Color.black, color, alpha);
            return pseudo;
        }

        public Color DefaultPlayerColorWithPseudoAlpha(float alpha)
        {
            return SetPseudoAlpha(DefaultPlayer, alpha);
        }

        public Color DefaultPlayerAdditionalColorPseudoAlpha()
        {
            return SetPseudoAlpha(DefaultPlayer, Player.DefaultAdditionalRatio);
        }

        public Color DefaultPlayerAdditionalColorRealAlpha()
        {
            return DefaultPlayer.WithAlpha(Player.DefaultAdditionalRatio);
        }
    }
}


namespace Assets.ColorManagers.SubManagers
{
    [Serializable]
    public struct PlayerColors
    {
        public float DefaultAdditionalRatio;
        public float OthelloAlpha;

        public Color Reflected;
        public Color Retribution;
        public Color Sentinel;
        public Color Void;
        public Color Parasite;
    }

    [Serializable]
    public struct EnemyColors
    {
        public EnemyGoreColors Gore;
        public Color Tank;
        public Color TankAlt;
        public Color RingEnemyRing;
        public Color LaserEnemyPrefire;
    }

    [Serializable]
    public struct EnemyGoreColors
    {
        public Color DefaultPrimary;
        public Color DefaultAlt;
    }

    [Serializable]
    public struct UIColors
    {
        public float VictimMarkerAlpha;
        public float MortarGuideAlpha;
    }

    [Serializable]
    public struct PickupColors
    {
        public Color Weapon;
        public Color Powerup;
        public Color DefaultWeaponPowerup;
        public Color WeaponLevel;
        public Color OneUp;
    }
}