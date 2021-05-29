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
            public MortarBalance Mortar;

            [Serializable]
            public struct PestControlBalance
            {
                public float BulletDamageBalanceBase;
                public float BaseChance;
                public float ChanceIncrease;

            }

            [Serializable]
            public struct MortarBalance
            {
                public PowerBalance Power;

                [Serializable]
                public struct PowerBalance
                {
                    public float Base;
                    public float Increase;
                }
            }
        }

        #endregion OnFire

        #region OnHit

        [Serializable]
        public struct OnHitBalance
        {
            public ShrapnelBalance Shrapnel;
            public MetronomeBalance Metronome;
            public CollectivePunishmentBalance CollectivePunishment;
            public SmiteBalance Smite;

            [Serializable]
            public struct ShrapnelBalance
            {
                public SpawnChanceBalance SpawnChance;

                [Serializable]
                public struct SpawnChanceBalance
                {
                    public float Base;
                    public float Increase;
                }
            }

            [Serializable]
            public struct MetronomeBalance
            {
                public DamageBalance Damage;

                [Serializable]
                public struct DamageBalance
                {
                    public float BaseValue;
                    public float ExponentBase;
                    public float Scale;
                }
            }

            [Serializable]
            public struct CollectivePunishmentBalance
            {
                public PowerBalance Power;

                [Serializable]
                public struct PowerBalance
                {
                    public float Base;
                    public float Increase;
                }
            }

            [Serializable]
            public struct SmiteBalance
            {
                public ChanceBalance Chance;

                [Serializable]
                public struct ChanceBalance
                {
                    public float Base;
                    public float Increase;
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
                    public float Increase;
                }

                [Serializable]
                public struct SizeScaleBalance
                {
                    public float Base;
                    public float Increase;
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
                    public float Increase;
                }

                [Serializable]
                public struct SizeScaleBalance
                {
                    public float Base;
                    public float Increase;
                }
            }
        }

        #endregion OnGetHit

        #region OnLevelUp

        [Serializable]
        public struct OnLevelUpBalance
        {
            public OthelloBalance Othello;
            public FireSpeedBalance FireSpeed;
            public MonsoonBalance Monsoon;
            public SentinelBalance Sentinel;
            public ParapetBalance Parapet;

            [Serializable]
            public struct OthelloBalance
            {
                public DamageBalance Damage;
                //public FireSpeedBalance FireSpeed;

                [Serializable]
                public struct DamageBalance
                {
                    public float Base;
                    public float Increase;
                }

                //[Serializable]
                //public struct FireSpeedBalance
                //{
                //    public float Base;
                //    public float Max;
                //}
            }

            [Serializable]
            public struct FireSpeedBalance
            {
                public float Base;
                public float Increase;
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
                public float Radius;
                public RespawnIntervalBalance RespawnInterval;
                //public WorldDistanceBalance WorldDistance;

                [Serializable]
                public struct RespawnIntervalBalance
                {
                    public float Base;
                    public float ScalePerLevel;
                }

                //[Serializable]
                //public struct WorldDistanceBalance
                //{
                //    public float Base;
                //    public float MaxValue;
                //}
            }

            [Serializable]
            public struct ParapetBalance
            {
                public HeightBalance Height;
                public ScaleBalance Scale;

                [Serializable]
                public struct HeightBalance
                {
                    public float Base;
                    public float IncreasePerLevel;
                }

                [Serializable]
                public struct ScaleBalance
                {
                    public Vector2 Base;
                    public Vector2 IncreasePerLevel;
                }
            }
        }

        #endregion OnLevelUp

        #region Passive

        [Serializable]
        public struct PassiveBalance
        {
            public InfernoBalance Inferno;
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
                    public float ScalePerLevel;
                }

                [Serializable]
                public struct DamageBalance
                {
                    public float Base;
                    public float Increase;
                }
            }

            [Serializable]
            public struct VictimBalance
            {
                public float TouchRadius;
                public FireSpeedBalance FireSpeed;
                public DamageBalance Damage;

                [Serializable]
                public struct FireSpeedBalance
                {
                    public float Base;
                    public float ScalePerLevel;
                }

                [Serializable]
                public struct DamageBalance
                {
                    public float Base;
                    public float Increase;
                }
            }
        }

        #endregion Passive
    }
}
