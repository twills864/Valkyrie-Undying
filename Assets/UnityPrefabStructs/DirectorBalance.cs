using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets
{
    [Serializable]
    public struct DirectorBalance
    {
        public DifficultyInfo Difficuly;
        public EnemyDropInfo EnemyDrops;
        public SpawnRateInfo SpawnRate;

        [Serializable]
        public struct DifficultyInfo
        {
            public float InitialDifficultyRatio;
            public float DifficultyRatioStep;
        }

        [Serializable]
        public struct EnemyDropInfo
        {
            public float BaseEnemyPowerupDropChance;

            public float WeaponLevelOverrideChanceFlatAddition;
        }


        [Serializable]
        public struct SpawnRateInfo
        {
            public int InitialTargetEnemiesOnScreen;

            public float InitialSpawnTime;

            [Obsolete(Constants.ObsoleteConstants.SpawnRampOverhaul)]
            public float SpawnRateSlowStartInit;
            [Obsolete(Constants.ObsoleteConstants.SpawnRampOverhaul)]
            public float SpawnRateSlowStartScaleDurationSeconds;
        }
    }
}
