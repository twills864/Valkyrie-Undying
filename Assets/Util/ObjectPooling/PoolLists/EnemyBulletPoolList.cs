using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Bullets.EnemyBullets;
using Assets.Enemies;
using Assets.EnemyBullets;
using UnityEngine;

namespace Assets.Util.ObjectPooling
{
    /// <inheritdoc/>
    public class EnemyBulletPoolList : PoolList<EnemyBullet>
    {
        [SerializeField]
        private BasicEnemyBullet BasicPrefab;
        [SerializeField]
        private TankEnemyBullet TankPrefab;

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
