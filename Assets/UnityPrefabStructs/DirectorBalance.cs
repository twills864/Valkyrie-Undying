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
        public EnemyDropInfo EnemyDrops;
        public SpawnRateInfo SpawnRate;

        [Serializable]
        public struct EnemyDropInfo
        {
            public float BaseEnemyPowerupDropChance;

            public float WeaponLevelOverrideChanceFlatAddition;

            public float OneUpOverrideChance;
        }


        [Serializable]
        public struct SpawnRateInfo
        {
            public int InitialTargetEnemiesOnScreen;

            public float InitialSpawnTime;

            public float SpawnRateSlowStartInit;
            public float SpawnRateSlowStartScaleDurationSeconds;
        }
    }
}
