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

        public OnDefaultWeaponFireBalance OnDefaultWeaponFire;
        public OnDefaultWeaponHitBalance OnDefaultWeaponHit;
        public OnDefaultWeaponKillBalance OnDefaultWeaponKill;
        public OnDefaultWeaponLevelUpBalance OnDefaultWeaponLevelUp;

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
            public ReboundBalance Rebound;

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

            [Serializable]
            public struct ReboundBalance
            {
                public ChanceBalance Chance;
                public PowerBalance Power;

                [Serializable]
                public struct ChanceBalance
                {
                    public float Base;
                    public float Increase;
                }

                [Serializable]
                public struct PowerBalance
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
            public JettisonBalance Jettison;

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
                public VariantFireSpeedBalance VariantFireSpeed;
                public DamageBalance Damage;

                [Serializable]
                public struct VariantFireSpeedBalance
                {
                    public float FireSpeed;
                    public float Variance;
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

            [Serializable]
            public struct JettisonBalance
            {
                public float ScaleXPerLevel;
                public float ScaleTime;
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
                public MaxDamageBalance MaxDamage;

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

                [Serializable]
                public struct MaxDamageBalance
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


        #region OnDefaultWeaponFire

        [Serializable]
        public struct OnDefaultWeaponFireBalance
        {
            public PlaceholderBalance Placeholder;
            public SnakeBiteBalance SnakeBite;

            [Serializable]
            public struct PlaceholderBalance
            {
                public float Placeholder;
            }

            [Serializable]
            public struct SnakeBiteBalance
            {
                public float Angle;
                public int PoisonDamage;
                public float FireSpeedRatio;
            }
        }

        #endregion OnDefaultWeaponFire

        #region OnDefaultWeaponHit

        [Serializable]
        public struct OnDefaultWeaponHitBalance
        {
            public ReboundBalance Rebound;
            public SplinterBalance Splinter;

            // Split doesn't need prefab adjustment
            //public SplitBalance Split;

            [Serializable]
            public struct ReboundBalance
            {
                public float AngleInDegrees;
            }

            [Serializable]
            public struct SplinterBalance
            {
                public float AngleInDegrees;
            }
        }

        #endregion OnDefaultWeaponHit

        #region OnDefaultWeaponKill

        [Serializable]
        public struct OnDefaultWeaponKillBalance
        {
            public PlaceholderBalance Placeholder;

            [Serializable]
            public struct PlaceholderBalance
            {
                public float Placeholder;
            }
        }

        #endregion OnDefaultWeaponKill

        #region OnDefaultWeaponLevelUp

        [Serializable]
        public struct OnDefaultWeaponLevelUpBalance
        {
            public PlaceholderBalance Placeholder;

            // PiercingRounds doesn't need prefab adjustment
            //public PiercingRoundsBalance PiercingRounds;

            public AugmentedRoundsBalance AugmentedRounds;
            public InfestedRoundsBalance InfestedRounds;
            public VenomousRoundsBalance VenomousRounds;

            [Serializable]
            public struct PlaceholderBalance
            {
                public float Placeholder;
            }

            [Serializable]
            public struct AugmentedRoundsBalance
            {
                public SizeBalance Size;
                public DamageBalance Damage;
                public SpeedBalance Speed;
                public ParticlesBalance Particles;

                [Serializable]
                public struct SizeBalance
                {
                    public float Base;
                    public float Increase;
                }

                [Serializable]
                public struct DamageBalance
                {
                    public float Base;
                    public float Increase;
                }

                [Serializable]
                public struct SpeedBalance
                {
                    public float Base;
                    public float Increase;
                }

                [Serializable]
                public struct ParticlesBalance
                {
                    public float Base;
                    public float Increase;
                }
            }

            [Serializable]
            public struct InfestedRoundsBalance
            {
                public NumParasitesBalance NumParasites;

                [Serializable]
                public struct NumParasitesBalance
                {
                    public float Base;
                    public float Increase;
                }
            }

            [Serializable]
            public struct VenomousRoundsBalance
            {
                public DamageBalance PoisonDamage;

                [Serializable]
                public struct DamageBalance
                {
                    public float Base;
                    public float Increase;
                }
            }
        }

        #endregion OnLevelUp
    }
}
