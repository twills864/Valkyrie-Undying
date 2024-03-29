using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Bullets.EnemyBullets;
using Assets.ColorManagers;
using Assets.Enemies;
using Assets.EnemyBullets;
using Assets.Util;
using UnityEngine;

namespace Assets.ObjectPooling
{
    /// <summary>
    /// A PoolList that contains EnemyBullet pools.
    /// </summary>
    /// <inheritdoc/>
    public class EnemyBulletPoolList : PoolList<EnemyBullet>
    {
#pragma warning disable 0414

        [SerializeField]
        private BasicEnemyBullet BasicPrefab = null;
        [SerializeField]
        private TankEnemyBullet TankPrefab = null;
        [SerializeField]
        private RingEnemyBullet RingPrefab = null;
        [SerializeField]
        private CradleEnemyBullet CradlePrefab = null;
        [SerializeField]
        private LaserEnemyBullet LaserPrefab = null;
        [SerializeField]
        private WaspEnemyBullet WaspPrefab = null;
        [SerializeField]
        private NomadEnemyBullet NomadPrefab = null;

#pragma warning restore 0414

        protected override Color GetDefaultColor(in ColorManager colorManager)
            => colorManager.DefaultEnemy;

        protected override void OnInitSprites(in ColorManager colorManager)
        {
            TankPrefab.SpriteColor = colorManager.Enemy.Tank;
            LaserPrefab.PrefireColor = colorManager.Enemy.LaserEnemyPrefire;
            //LaserSpawnerPrefab.SpriteColor = colorManager.Enemy.LaserEnemyLaserPrefire;
        }

        public EnemyBullet[] GetPestControlTargets(int numToGet)
        {
            var activeBullets = GetAllActiveObjects();

            var highBullets = new List<EnemyBullet>();
            var lowBullets = new List<EnemyBullet>();

            float worldBottomY = SpaceUtil.WorldMap.Bottom.y;
            foreach (var bullet in activeBullets)
            {
                var posY = bullet.transform.position.y;

                if (posY > Enemy.FireHeightFloor)
                    highBullets.Add(bullet);
                else if(posY > worldBottomY)
                    lowBullets.Add(bullet);
            }

            EnemyBullet[] ret;

            // Enough bullets in highBullets
            if (highBullets.Count >= numToGet)
                ret = RandomUtil.GetUpToXRandomElements(highBullets, numToGet);

            // Enough bullets between the two lists
            else if (highBullets.Count + lowBullets.Count >= numToGet)
            {
                ret = new EnemyBullet[numToGet];
                Array.Copy(highBullets.ToArray(), ret, highBullets.Count);

                int numRemaining = numToGet - highBullets.Count;

                if (numRemaining != lowBullets.Count)
                    RandomUtil.Shuffle(lowBullets);

                for (int i = 0; i < numRemaining; i++)
                {
                    ret[i + highBullets.Count] = lowBullets[i];
                }
            }

            // Not enough between the two lists
            else
                ret = highBullets.Concat(lowBullets).ToArray();

            return ret;
        }
    }
}
