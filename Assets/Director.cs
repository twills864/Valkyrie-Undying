﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Enemies;
using Assets.ObjectPooling;
using Assets.Util;
using UnityEngine;

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

        private static List<Enemy> ActiveEnemies = new List<Enemy>();
        private static EnemyPoolList EnemyPoolList { get; set; }

        private static int EnemiesSpawned { get; set; }
        public static int EnemyHealthIncrease => EnemiesSpawned;


        public static void Init()
        {
            EnemySpawnTimer.ActivateSelf();
            EnemyPoolList = PoolManager.Instance.EnemyPool;

            //DebugUI.SetDebugLabel("AE", DebugActiveEnemiesString);
        }

        private static string DebugActiveEnemiesString()
        {
            var countedEnemies = ActiveEnemies.Where(x => x.InfluencesDirectorGameBalance).ToList();
            string enemies = String.Join("\r\n", countedEnemies);
            string message = $"{countedEnemies.Count} {enemies}";
            return message;
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

        public static void EnemySpawned(Enemy enemy)
        {
            ActiveEnemies.Add(enemy);

#if UNITY_EDITOR
            enemy.ActiveInDirector = true;
#endif
        }

        public static void EnemyDeactivated(Enemy enemy)
        {
#if UNITY_EDITOR
            if(!enemy.ActiveInDirector)
            {
                const string Message = "ERROR: DEACTIVATING ENEMY THAT WAS NOT ACCOUNTED FOR BY DIRECTOR.";
                enemy.Log(Message);
                GameManager.Instance.CreateFleetingText(Message, enemy.transform.position);
            }

            enemy.ActiveInDirector = false;
            enemy.DespawnHandledByDirector = true;
#endif

            ActiveEnemies.Remove(enemy);

            if (enemy.InfluencesDirectorGameBalance)
            {
                if (enemy.WasKilled)
                    EnemyKilled(enemy);
                else
                    EnemyEscaped(enemy);
            }
        }

        private static void EnemyKilled(Enemy enemy)
        {
            // TODO: Handle difficulty
        }

        private static void EnemyEscaped(Enemy enemy)
        {
            // TODO: Handle difficulty
        }






        // Can't use standard IEnumerable because enemies may get removed during enumeration.
        public static IEnumerable<Enemy> GetAllActiveEnemies()
        {
            for (int i = ActiveEnemies.Count - 1; i >= 0; i--)
                yield return ActiveEnemies[i];
        }

        #region TryGetRandomEnemy

        public static bool TryGetRandomEnemy(out Enemy enemy)
        {
            var ret = RandomUtil.TryGetRandomElement(ActiveEnemies, out enemy);
            return ret;
        }
        public static bool TryGetRandomEnemyExcluding(Enemy exclusion, out Enemy enemy)
        {
            var potentials = ActiveEnemies
                .Where(x => x != exclusion);
            var ret = RandomUtil.TryGetRandomElement(potentials, out enemy);
            return ret;
        }

        #endregion TryGetRandomEnemy
    }
}
