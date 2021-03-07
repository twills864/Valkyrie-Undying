using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Bullets;
using Assets.Bullets.PlayerBullets;
using Assets.Enemies;
using Assets.Util;
using UnityEngine;

namespace Assets.Powerups.Balance
{
    /// <summary>
    /// A collection of values to use in the calculation of powerup power.
    ///
    /// Designed for readability in the GameManager Unity editor object
    /// at the cost of minor C# code readability.
    /// </summary>
    [Serializable]
    public struct PowerupBalanceManager
    {
        public OnFireBalance OnFire;
        public OnHitBalance OnHit;
        public OnKillBalance OnKill;
        public OnGetHitBalance OnGetHit;
        public OnLevelUpBalance OnLevelUp;
        public PassiveBalance Passive;

        #region OnFire

        [Serializable]
        public struct OnFireBalance
        {
            public PestControlBalance PestControl;

            [Serializable]
            public struct PestControlBalance
            {
                public float ExponentRatio;
                public float MaxValue;
            }
        }

        #endregion OnFire

        #region OnHit

        [Serializable]
        public struct OnHitBalance
        {
            public ShrapnelBalance Shrapnel;

            [Serializable]
            public struct ShrapnelBalance
            {
                public SpawnChanceBalance Spawn;

                [Serializable]
                public struct SpawnChanceBalance
                {
                    public float BaseChance;
                    public float ChanceIncrease;
                }
            }
        }

        #endregion OnHit

        #region OnKill

        [Serializable]
        public struct OnKillBalance
        {
            public BloodlustBalance Bloodlust;
            public VoidBalance Void;

            [Serializable]
            public struct BloodlustBalance
            {
                public DurationBalance Duration;
                public SpeedScaleBalance SpeedScale;

                [Serializable]
                public struct DurationBalance
                {
                    public float Base;
                    public float IncreasePerLevel;
                }

                [Serializable]
                public struct SpeedScaleBalance
                {
                    public float Base;
                    public float Increase;
                }
            }

            [Serializable]
            public struct VoidBalance
            {
                public DurationBalance Duration;
                public SizeScaleBalance SizeScale;

                [Serializable]
                public struct DurationBalance
                {
                    public float Base;
                    public float Max;
                }

                [Serializable]
                public struct SizeScaleBalance
                {
                    public float Base;
                    public float Max;
                }
            }
        }

        #endregion OnKill

        #region OnGetHit

        [Serializable]
        public struct OnGetHitBalance
        {
            public RetributionBalance Retribution;

            [Serializable]
            public struct RetributionBalance
            {
                public DurationBalance Duration;
                public SizeScaleBalance SizeScale;

                [Serializable]
                public struct DurationBalance
                {
                    public float Base;
                    public float Max;
                }

                [Serializable]
                public struct SizeScaleBalance
                {
                    public float InitialValue;
                    public float Base;
                    public float Max;
                }
            }
        }

        #endregion OnGetHit

        #region OnLevelUp

        [Serializable]
        public struct OnLevelUpBalance
        {
            public OthelloBalance Othello;
            public FireSpeedBalance Firespeed;

            [Serializable]
            public struct OthelloBalance
            {
                public DamageBalance Damage;
                public FireSpeedBalance FireSpeed;

                [Serializable]
                public struct DamageBalance
                {
                    public float Base;
                    public float Increase;
                }

                [Serializable]
                public struct FireSpeedBalance
                {
                    public float Base;
                    public float Max;
                }
            }

            [Serializable]
            public struct FireSpeedBalance
            {
                public float Base;
                public float Increase;
            }
        }

        #endregion OnLevelUp

        #region Passive

        [Serializable]
        public struct PassiveBalance
        {
            public InfernoBalance Inferno;
            public MonsoonBalance Monsoon;
            public SentinelBalance Sentinel;
            public VictimBalance Victim;

            [Serializable]
            public struct InfernoBalance
            {
                public FireSpeedBalance FireSpeed;
                public DamageBalance Damage;

                [Serializable]
                public struct FireSpeedBalance
                {
                    public float Base;
                    public float Increase;
                }

                [Serializable]
                public struct DamageBalance
                {
                    public float Base;
                    public float IncreasePerLevel;
                }
            }

            [Serializable]
            public struct MonsoonBalance
            {
                public FireSpeedBalance FireSpeed;
                public DamageBalance Damage;

                [Serializable]
                public struct FireSpeedBalance
                {
                    public float Base;
                    public float Increase;
                }

                [Serializable]
                public struct DamageBalance
                {
                    public float Base;
                    public float IncreasePerLevel;
                }
            }

            [Serializable]
            public struct SentinelBalance
            {
                public RespawnIntervalBalance RespawnInterval;
                public WorldDistanceBalance WorldDistance;

                [Serializable]
                public struct RespawnIntervalBalance
                {
                    public float Base;
                    public float Increase;
                }

                [Serializable]
                public struct WorldDistanceBalance
                {
                    public float Base;
                    public float MaxValue;
                }
            }

            [Serializable]
            public struct VictimBalance
            {
                public FireSpeedBalance FireSpeed;
                public DamageBalance Damage;

                [Serializable]
                public struct FireSpeedBalance
                {
                    public float Base;
                    public float Scale;
                }

                [Serializable]
                public struct DamageBalance
                {
                    public float Base;
                    public float IncreasePerLevel;
                }
            }
        }

        #endregion Passive
    }
}
