using Assets.Enemies;
using UnityEngine;

namespace Assets.Util.ObjectPooling
{
    /// <inheritdoc/>
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
