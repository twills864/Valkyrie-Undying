using Assets.Bullets;
using Assets.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util.ObjectPooling
{
    public class EnemyPoolList : PoolList<Enemy>
    {
        [SerializeField]
        private BasicEnemy BasicPrefab;
        [SerializeField]
        private TankEnemy TankPrefab;

        public Enemy GetRandomEnemy()
        {
            var randomPool = RandomUtil.RandomElement(Pools);
            var ret = randomPool.Get();
            return ret;
        }
    }
}
