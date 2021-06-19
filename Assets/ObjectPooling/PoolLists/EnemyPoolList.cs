using System.Collections.Generic;
using Assets.Enemies;
using UnityEngine;
using System.Linq;
using Assets.Util;
using System;
using Assets.ColorManagers;
using Assets.Constants;

namespace Assets.ObjectPooling
{
    /// <inheritdoc/>
    public class EnemyPoolList : PoolList<Enemy>
    {
#pragma warning disable 0414

        [SerializeField]
        private BasicEnemy BasicPrefab = null;
        [SerializeField]
        private TankEnemy TankPrefab = null;
        [SerializeField]
        private RingEnemy RingEnemyPrefab = null;
        [SerializeField]
        private RingEnemyRing RingEnemyRingPrefab = null;
        [SerializeField]
        private CradleEnemy CradleEnemyPrefab = null;
        [SerializeField]
        private LaserEnemy LaserEnemyPrefab = null;
        [SerializeField]
        private WaspEnemy WaspPrefab = null;

#pragma warning restore 0414

        private ObjectPool<Enemy>[] SpawnableEnemyPools { get; set; }

        public ObjectPool<Enemy>[] GetSpawnableEnemyPools()
        {
            ObjectPool<Enemy>[] spawnableEnemypools = new ObjectPool<Enemy>[SpawnableEnemyPools.Length];
            SpawnableEnemyPools.CopyTo(spawnableEnemypools, 0);
            return spawnableEnemypools;
        }

        private ObjectPool<Enemy> OverridePool => GameManager.OverrideEnemyType != null &&
            PoolMap.TryGetValue(GameManager.OverrideEnemyType, out var ret) ? ret : null;

        protected override Color GetDefaultColor(in ColorManager colorManager)
            => Color.white; // colorManager.DefaultEnemy;

        protected override void OnInitSprites(in ColorManager colorManager)
        {
            var enemy = colorManager.Enemy;

            bool showGore = PlayerPrefsUtil.GetBoolFromPrefs(PlayerPrefsUtil.ToggleGoreKey, false);
            if (!showGore)
                InitRealParticles(in colorManager);
            else
                InitGoreParticles(in colorManager);

            RingEnemyRingPrefab.ParticleColor = enemy.RingEnemyRing;
            RingEnemyRingPrefab.ParticleColorAlt = enemy.RingEnemyRing;
            RingEnemyRingPrefab.SpriteColor = enemy.RingEnemyRing;
        }

        private void InitRealParticles(in ColorManager colorManager)
        {
            var enemy = colorManager.Enemy;

            foreach (var prefab in AllPrefabs)
            {
                prefab.ParticleColor = colorManager.DefaultEnemy;
                prefab.ParticleColorAlt = colorManager.DefaultEnemyAlt;
            }

            TankPrefab.ParticleColor = enemy.Tank;
            TankPrefab.ParticleColorAlt = enemy.TankAlt;
        }

        private void InitGoreParticles(in ColorManager colorManager)
        {
            var enemy = colorManager.Enemy;

            foreach (var prefab in AllPrefabs)
            {
                prefab.ParticleColor = enemy.Gore.DefaultPrimary;
                prefab.ParticleColorAlt = enemy.Gore.DefaultAlt;
            }

            TankPrefab.ParticleColor = enemy.Gore.DefaultPrimary;
            TankPrefab.ParticleColorAlt = enemy.Gore.DefaultAlt;
        }

        protected override void OnPoolMapSet()
        {
            Type[] exclusionTypes = new Type[]
            {
                typeof(RingEnemyRing)
            };


            SpawnableEnemyPools = PoolMap.Where(x => !exclusionTypes.Contains(x.Key))
                .Select(x => x.Value).ToArray();
        }

        public Enemy GetRandomEnemy()
        {
            ObjectPool<Enemy> pool = OverridePool;

            if(pool == null)
                pool = RandomUtil.RandomElement(SpawnableEnemyPools);

            var ret = pool.Get();
            ret.transform.position = SpaceUtil.RandomEnemySpawnPosition(ret);
            ret.OnSpawn();
            return ret;
        }

        // Currently only used for debugging
        public TEnemy SpawnSpecificEnemy<TEnemy>() where TEnemy : Enemy
        {
            var ret = Get<TEnemy>();
            ret.transform.position = SpaceUtil.RandomEnemySpawnPosition(ret);
            ret.OnSpawn();
            return ret;
        }
    }
}
