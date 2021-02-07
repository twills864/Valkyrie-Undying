using System.Collections.Generic;
using Assets.Enemies;
using UnityEngine;
using System.Linq;

namespace Assets.Util.ObjectPooling
{
    /// <inheritdoc/>
    public class EnemyPoolList : PoolList<Enemy>
    {
        [SerializeField]
        private BasicEnemy BasicPrefab;
        [SerializeField]
        private TankEnemy TankPrefab;

        public Enemy SpawnRandomEnemy()
        {
            var randomPool = RandomUtil.RandomElement(Pools);
            var ret = randomPool.Get();
            return ret;
        }
    }
}
