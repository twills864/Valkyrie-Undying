using System;
using System.Linq;
using Assets.Util;
using Assets.Constants;
using Assets.GameTasks;
using Assets.ObjectPooling;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Bullets.PlayerBullets
{
    /// <summary>
    /// Spawns one Parasite bullet each frame at a set of specified locations.
    /// </summary>
    /// <inheritdoc/>
    public class ParasiteSpawnManager
    {
        private List<SpawnPoint> SpawnPoints { get;}

        public ParasiteSpawnManager()
        {
            SpawnPoints = new List<SpawnPoint>();
        }

        public void Update()
        {
            for(int i = SpawnPoints.Count - 1; i >= 0; i--)
            {
                if (SpawnPoints[i].SpawnDepletes())
                    SpawnPoints.RemoveAt(i);
            }
        }

        public void AddParasites(Vector2 position, int numberToAdd)
        {
            var add = new SpawnPoint(position, numberToAdd);
            SpawnPoints.Add(add);
        }


        private class SpawnPoint
        {
            public Vector2 Point;
            public int NumberToSpawn;

            public SpawnPoint(Vector2 point, int numberToSpawn)
            {
                Point = point;
                NumberToSpawn = numberToSpawn;
            }

            public bool SpawnDepletes()
            {
                var bullet = PoolManager.Instance.BulletPool.Get<ParasiteBullet>(Point);
                bullet.OnSpawn();

                if (NumberToSpawn != 1)
                {
                    NumberToSpawn--;
                    return false;
                }
                else
                    return true;
            }
        }
    }
}