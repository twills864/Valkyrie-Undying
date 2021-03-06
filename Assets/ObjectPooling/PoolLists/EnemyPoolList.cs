using System.Collections.Generic;
using Assets.Enemies;
using UnityEngine;
using System.Linq;
using Assets.Util;
using System;
using Assets.ColorManagers;

namespace Assets.ObjectPooling
{
    /// <inheritdoc/>
    public class EnemyPoolList : PoolList<Enemy>
    {
        [SerializeField]
        private BasicEnemy BasicPrefab;
        [SerializeField]
        private TankEnemy TankPrefab;
        [SerializeField]
        private RingEnemy RingEnemyPrefab;
        [SerializeField]
        private RingEnemyRing RingEnemyRingPrefab;


        private ObjectPool<Enemy>[] RandomEnemyPools { get; set; }

        public Type OverrideEnemyType => null; // DebugUtil.GetOverrideEnemyType<RingEnemy>();
        private ObjectPool<Enemy> OverridePool => OverrideEnemyType != null &&
            PoolMap.TryGetValue(OverrideEnemyType, out var ret) ? ret : null;

        protected override Color GetDefaultColor(ColorManager colorManager)
            => colorManager.DefaultEnemy;

        protected override void OnInitSprites(ColorManager colorManager)
        {
            var enemy = colorManager.Enemy;

            TankPrefab.SpriteColor = enemy.Tank;
            RingEnemyRingPrefab.SpriteColor = enemy.RingEnemyRing;
        }

        protected override void OnPoolMapSet()
        {
            Type[] exclusionTypes = new Type[]
            {
                typeof(RingEnemyRing)
            };


            RandomEnemyPools = PoolMap.Where(x => !exclusionTypes.Contains(x.Key))
                .Select(x => x.Value).ToArray();
        }

        public Enemy GetRandomEnemy()
        {
            ObjectPool<Enemy> pool = OverridePool;

            if(pool == null)
                pool = RandomUtil.RandomElement(RandomEnemyPools);

            var ret = pool.Get();
            ret.transform.position = SpaceUtil.RandomEnemySpawnPosition(ret);
            ret.OnSpawn();
            return ret;
        }
    }
}
