using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FireStrategyManagers
{
    /// <summary>
    /// A collection of values to use in the calculation of fire speeds.
    ///
    /// The player's fire speeds are represented as ratios of the base fire speed,
    /// in order to easily change the pace of player firing.
    ///
    /// Designed for readability in the GameManager Unity editor object
    /// at the cost of minor C# code readability.
    /// </summary>
    [Serializable]
    public struct PlayerFireStrategyManager
    {
        public float BaseFireSpeed;
        public PlayerRatio PlayerRatios;
        //public EnemySpeed EnemySpeeds;

        [Serializable]
        public struct PlayerRatio
        {
            public float Shotgun;
            public BurstInfo Burst;
            public float Bounce;
            public float Atom;
            public float Spread;
            public float Flak;
            public float Trampoline;
            public float Wormhole;
            public float Gatling;
            public BfgInfo Bfg;
            public float OneManArmy;
            public float DeadlyDiamond;
            public float Reflect;

            [Serializable]
            public struct BurstInfo
            {
                public float FireRatio;
                public float ReloadRatio;
            }

            [Serializable]
            public struct BfgInfo
            {
                public float ChargeRatio;
                public float LaserRatio;
            }
        }

        //[Serializable]
        //public struct EnemySpeed
        //{
        //    public FireSpeedVariance Debug;
        //    public FireSpeedVariance Basic;
        //    public FireSpeedVariance Tank;
        //    public FireSpeedVariance Ring;


        //    [Serializable]
        //    public struct FireSpeedVariance
        //    {
        //        public float Speed;
        //        public float Variance;
        //    }
        //}
    }
}


namespace Assets.FireStrategyManagers.SubManagers
{
}
