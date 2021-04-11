using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.ObjectPooling;
using Assets.Util;

namespace Assets
{
    /// <summary>
    /// The entity that directs the flow of the game, including enemy spawns,
    /// difficulty curves, powerup drops, etc.
    /// </summary>
    public static class Director
    {
        public static float TotalTime { get; private set; }

        private static LoopingFrameTimer EnemySpawnTimer = new LoopingFrameTimer(3.0f); // new InactiveLoopingFrameTimer();

        private static EnemyPoolList EnemyPoolList { get; set; }

        private static int EnemiesSpawned { get; set; }
        public static int EnemyHealthIncrease => EnemiesSpawned;


        public static void Init()
        {
            EnemySpawnTimer.ActivateSelf();
            EnemyPoolList = PoolManager.Instance.EnemyPool;
        }

        public static void RunFrame(float deltaTime, float realDeltaTime)
        {
            TotalTime += deltaTime;

            if (EnemySpawnTimer.UpdateActivates(deltaTime))
                SpawnEnemy();
        }

        public static void SpawnEnemy()
        {
            var enemy = EnemyPoolList.GetRandomEnemy();


            EnemiesSpawned++;
        }


    }
}
